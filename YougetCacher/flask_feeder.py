# *-encoding: utf8 -*
import mimetypes
import os

import sys

import re
from flask import Flask, render_template, request, make_response, Response
from werkzeug.routing import BaseConverter

app = Flask('my vi host')

MB = 1 << 20
BUFF_SIZE = 10 * MB


@app.route("/")
def index():
    # return "Hello World!"
    return render_template('play.html')


def get_range(request):
    range = request.headers.get('Range')
    # LOG.info('Requested: %s', range)
    m = re.match('bytes=(?P<start>\d+)-(?P<end>\d+)?', range)
    if m:
        start = m.group('start')
        end = m.group('end')
        start = int(start)
        if end is not None:
            end = int(end)
        return start, end
    else:
        return 0, None


def partial_response(path, start, end=None):
    print(str.format('Requested: {0}, {1}', str(start), str(end)))
    file_size = os.path.getsize(path)

    # Determine (end, length)
    if end is None:
        end = start + BUFF_SIZE - 1
    end = min(end, file_size - 1)
    end = min(end, start + BUFF_SIZE - 1)
    length = end - start + 1

    # Read file
    with open(path, 'rb') as fd:
        fd.seek(start)
        bytes = fd.read(length)
    assert len(bytes) == length

    response = Response(bytes, 206, mimetype=mimetypes.guess_type(path)[0], direct_passthrough=True,)
    response.headers.add('Content-Range', 'bytes {0}-{1}/{2}'.format(start, end, file_size,),)
    response.headers.add('Accept-Ranges', 'bytes')
    return response


@app.route("/play/<resource>/")
@app.route("/play/<resource>")
def get_resource(resource):
    if resource.endswith('.webm'):
        req_path = os.path.join('./', resource)
        if not os.path.isfile(req_path):
            return make_response('not found', 404)

    start, end = get_range(request)
    return partial_response(req_path, start, end)
    # try:
    #     print(str.format("if_range: {0}, range-start: {1}, range-end: {2}",
    #                      str(request.if_range),
    #                      str(request.range.ranges[0][0]),
    #                      str(request.range.ranges[0][1])))
    #     req_start = request.range.ranges[0][0]
    #     req_end = request.range.ranges[0][1]
    # except:
    #     pass
    #
    # if resource.endswith('.webm'):
    #     req_file = os.req_path.join('./', resource)
    #     if not os.req_path.isfile(req_file):
    #         return make_response('not found', 404)
    #
    #     if req_start == 0 and req_end is None:
    #         resp = Response()
    #         resp.content_length = os.req_path.getsize(req_file)
    #         resp.accept_ranges = 'bytes'
    #         return resp
    #     else:
    #         with open(req_file, mode='rb') as f:
    #             buf = f.read()
    #
    #     return buf
    #
    # else:
    #     return 'not supported resource type'


if __name__ == '__main__':
    app.run(debug=True, host='localhost', port=10988)
