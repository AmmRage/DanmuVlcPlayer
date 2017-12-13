# *-encoding: utf8 -*

from flask import *


app = Flask(__name__)

@app.route("/")
def index():
    return "Hello World!"


@app.route("/play/<resource>")
def get_resource(resource):
    print(str.format("if_range: {0}, range: {1}, max_content_length: {2}",
                     str(request.if_range),
                     str(request.range),
                     str(request.max_content_length)))

    return "Hello World!"


if __name__ == '__main__':
    app.run('http://localhost', 10234)
