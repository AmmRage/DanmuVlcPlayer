using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Linq;

using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.MediaFoundation;

using DXDevice = SharpDX.Direct3D11.Device;
using SharpDX;
using SharpDX.DXGI;
using SharpDX.Mathematics.Interop;
using SharpDX.Windows;
using Resource = SharpDX.Direct3D11.Resource;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using Format = SharpDX.DXGI.Format;
using PixelFormat = SharpDX.Direct2D1.PixelFormat;

namespace VlcDemo
{
    public class MfPlayer
    {
        #region attributes

        private readonly IList<string> SupportedFormatsExtensions;

        /// <summary>
        /// The event raised when MediaEngine is ready to play the music.
        /// </summary>
        private readonly ManualResetEvent EventReadyToPlay;

        /// <summary>
        /// Set when the music is stopped.
        /// </summary>
        private bool _isMusicStopped;

        /// <summary>
        /// The instance of MediaEngineEx
        /// </summary>
        private MediaEngineEx _mediaEngineEx;

        /// <summary>
        /// Our dx11 device
        /// </summary>
        private DXDevice _device;

        /// <summary>
        /// Our SwapChain
        /// </summary>
        private SwapChain _swapChain;

        /// <summary>
        /// DXGI Manager
        /// </summary>
        private DXGIDeviceManager _dxgiManager;

        private long ts;

        private MediaEngine mediaEngine;

        private Control playerForm;

        private SolidColorBrush brush;

        private RenderTarget target;

        private Surface surface;

        private TextLayout textLayout;

        private int w, h;

        private TextFormat textFormat;
        #endregion

        #region ctor & dector
        public MfPlayer()
        {
            this.EventReadyToPlay = new ManualResetEvent(false);
            this.SupportedFormatsExtensions = @".3g2, .3gp, .3gp2, .3gpp, .asf, .wma, .wmv, .aac, .adts, .avi, .mp3, .m4a, .m4v, .mov, .mp4, .sami, .smi, .wav"
                .Split(',').Select(e => e.Trim()).ToList();
        }
        ~MfPlayer()
        {
            try
            {
                if(this.mediaEngine.NativePointer != (IntPtr)0)
                    this.mediaEngine.Shutdown();
            }
            catch
            {
            }
            this._swapChain.Dispose();
            this._device.Dispose();
        }
        #endregion 

        #region private methods
        /// <summary>
        /// Called when [playback callback].
        /// </summary>
        /// <param name="playEvent">The play event.</param>
        /// <param name="param1">The param1.</param>
        /// <param name="param2">The param2.</param>
        private  void OnPlaybackCallback(MediaEngineEvent playEvent, long param1, int param2)
        {
            switch (playEvent)
            {
                case MediaEngineEvent.CanPlay:
                    this.EventReadyToPlay.Set();
                    break;
                case MediaEngineEvent.TimeUpdate:
                    break;
                case MediaEngineEvent.Error:
                case MediaEngineEvent.Abort:
                case MediaEngineEvent.Ended:
                    this._isMusicStopped = true;
                    break;
            }
        }

        /// <summary>
        /// Creates device with necessary flags for video processing
        /// </summary>
        /// <param name="manager">DXGI Manager, used to create media engine</param>
        /// <returns>Device with video support</returns>
        private DXDevice CreateDeviceForVideo(out DXGIDeviceManager manager)
        {
            //Device need bgra and video support
            var device = new DXDevice(DriverType.Hardware,
                DeviceCreationFlags.BgraSupport | DeviceCreationFlags.VideoSupport);

            //Add multi thread protection on device
            var mt = device.QueryInterface<DeviceMultithread>();
            mt.SetMultithreadProtected(true);

            //Reset device
            manager = new DXGIDeviceManager();
            manager.ResetDevice(device);

            return device;
        }

        /// <summary>
        /// Creates swap chain ready to use for video output
        /// </summary>
        /// <param name="dxdevice">DirectX11 device</param>
        /// <param name="handle">RenderForm Handle</param>
        /// <returns>SwapChain</returns>
        private SwapChain CreateSwapChain(DXDevice dxdevice, IntPtr handle)
        {
            //Walk up device to retrieve Factory, necessary to create SwapChain
            var dxgidevice = dxdevice.QueryInterface<SharpDX.DXGI.Device>();
            var adapter = dxgidevice.Adapter.QueryInterface<Adapter>();
            var factory = adapter.GetParent<SharpDX.DXGI.Factory1>();

            /*To be allowed to be used as video, texture must be of the same format (eg: bgra), and needs to be bindable are render target.
             * you do not need to create render target view, only the flag*/
            var sd = new SwapChainDescription()
            {
                BufferCount = 1,
                ModeDescription = new ModeDescription(0, 0, new Rational(60, 1), Format.B8G8R8A8_UNorm),
                IsWindowed = true,
                OutputHandle = handle,
                SampleDescription = new SampleDescription(1, 0),
                SwapEffect = SwapEffect.Discard,
                Usage = Usage.RenderTargetOutput,
                Flags = SwapChainFlags.None
            };

            return new SwapChain(factory, dxdevice, sd);
        }

        private ByteStream GetMediaByteStream(string file)
        {
            var extension = Path.GetExtension(file);
            if (this.SupportedFormatsExtensions.Any(f => f.Equals(extension, StringComparison.OrdinalIgnoreCase)))
            {
                // Opens the file
                var fileStream = File.Open(file, FileMode.Open);
                // Create a ByteStream object from it
                return new ByteStream(fileStream);
            }
            throw new Exception("Unsupported format");
        }

        #endregion

        /// <summary>
        /// Attach player to a form
        /// </summary>
        public void Attach(Control renderTarget)
        {
            //this.playerForm = new RenderForm();
            this.playerForm = renderTarget;
            // Initialize MediaFoundation
            MediaManager.Startup();
            //text
            var factoryDWrite = new SharpDX.DirectWrite.Factory();
            textFormat = new TextFormat(factoryDWrite, "consolas", 14)
            {
                TextAlignment = TextAlignment.Center,
                ParagraphAlignment = ParagraphAlignment.Center,
                WordWrapping = WordWrapping.NoWrap,                                
            };
            //SolidColorBrush SceneColorBrush = new SolidColorBrush(renderTarget2D, SharpDX.Color.Red);
            var clientRectangle = new RectangleF(0, 0, renderTarget.Width, renderTarget.Height);

            //play
            this._device = CreateDeviceForVideo(out this._dxgiManager);
            //// Creates the MediaEngineClassFactory
            var mediaEngineFactory = new MediaEngineClassFactory();
            //Assign our dxgi manager, and set format to bgra
            var attr = new MediaEngineAttributes
            {
                VideoOutputFormat = (int) Format.B8G8R8A8_UNorm,
                DxgiManager = this._dxgiManager
            };
            // Creates MediaEngine for AudioOnly 
            this.mediaEngine = new MediaEngine(mediaEngineFactory, attr);
            // Register our PlayBackEvent
            this.mediaEngine.PlaybackEvent += OnPlaybackCallback;
            // Query for MediaEngineEx interface
            this._mediaEngineEx = this.mediaEngine.QueryInterface<MediaEngineEx>();
            
            //Create our swapchain
            this._swapChain = CreateSwapChain(this._device, renderTarget.Handle);

            //Get DXGI surface to be used by our media engine
            var texture = Resource.FromSwapChain<Texture2D>(this._swapChain, 0);
            this.surface = texture.QueryInterface<Surface>();

            var properties = new RenderTargetProperties()
            {
                PixelFormat = new PixelFormat(Format.B8G8R8A8_UNorm, SharpDX.Direct2D1.AlphaMode.Ignore),
            };
            var factory = new SharpDX.Direct2D1.Factory();
            this.target = new RenderTarget(factory, this.surface, properties);
            this.brush = new SolidColorBrush(this.target, Color.Red);
            
            using (var fac = new SharpDX.DirectWrite.Factory())
            {
                this.textLayout = new TextLayout(fac, "SharpDX D2D1 - DWrite", textFormat, 100,
                    100);
            }
        }

        public void Play(string file)
        {
            Task.Factory.StartNew(() =>
            {
                var stream = GetMediaByteStream(file);
                // Creates an URL to the file
                var url = new Uri(file, UriKind.RelativeOrAbsolute);
                // Set the source stream
                this._mediaEngineEx.SetSourceFromByteStream(stream, url.AbsoluteUri);                
                try
                {
                    // Wait for MediaEngine to be ready
                    if (!this.EventReadyToPlay.WaitOne(1000))
                    {
                        Console.WriteLine("Unexpected error: Unable to play this file");
                    }
                    //Get our video size
                    this.mediaEngine.GetNativeVideoSize(out this.w, out this.h);
                    // Play
                    this._mediaEngineEx.Play();
                    var dstRect = new RawRectangle(0, 0, this.w, this.h);
                    var origVector = new Vector2(0, 0);
                  
                    using (var renderLoop = new RenderLoop(this.playerForm) { UseApplicationDoEvents = false })
                    {
                        while (true)
                        {
                            if (this.mediaEngine.OnVideoStreamTick(out this.ts))
                            {
                                //this.mediaEngine.TransferVideoFrame(this.surface, null, dstRect, null);
                            }
                            this.target.BeginDraw();

                            //this.target.DrawTextLayout(origVector, this.textLayout, this.brush, DrawTextOptions.None);
                            this.target.DrawText("123456", textFormat, new RawRectangleF(0, 0, 100, 100), this.brush);
                            this.target.EndDraw();
                            this._swapChain.Present(1, PresentFlags.UseDuration);
                        }
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            });
        }

        public void Play()
        {
        }

        public void Pause()
        {
        }

        public bool IsPlaying { get; private set; }

        public float Position {
            get { return (float) this._mediaEngineEx.CurrentTime; }
            set { this._mediaEngineEx.SetCurrentTimeEx(value, MediaEngineSeekMode.Normal); }
        }

        public void Stop()
        {
            this.mediaEngine.Shutdown();
        }
    }
}
