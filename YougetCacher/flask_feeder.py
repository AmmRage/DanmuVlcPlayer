# *-encoding: utf8 -*
import os

import sys
from flask import Flask, render_template, request, make_response, Response
from werkzeug.routing import BaseConverter

app = Flask('my vi host')


@app.route("/")
def index():
    # return "Hello World!"
    return render_template('play.html')


@app.route("/play/<resource>/")
def get_resource(resource):
    try:
        print(str.format("if_range: {0}, range-start: {1}, range-end: {2}",
                         str(request.if_range),
                         str(request.range.ranges[0][0]),
                         str(request.range.ranges[0][1])))
        req_start = request.range.ranges[0][0]
        req_end = request.range.ranges[0][1]
    except:
        pass

    if resource.endswith('.webm'):
        req_file = os.path.join('./', resource)
        if not os.path.isfile(req_file):
            return make_response('not found', 404)

        if req_start == 0 and req_end is None:
            resp = Response()
            resp.content_length = os.path.getsize(req_file)
            resp.accept_ranges = 'bytes'
            return resp
        else:
            with open(req_file, mode='rb') as f:
                buf = f.read()

        return buf

    else:
        return 'not supported resource type'


if __name__ == '__main__':
    app.run(debug=True, host='0.0.0.0', port=10987)
