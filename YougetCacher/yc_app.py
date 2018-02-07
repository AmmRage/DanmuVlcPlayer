# coding=utf-8

from gevent.wsgi import WSGIServer

from flask_feeder import app

if __name__ == '__main__':
    http_server = WSGIServer(('', 10988), app)
    http_server.serve_forever()


