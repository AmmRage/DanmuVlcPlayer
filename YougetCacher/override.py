
import io
import os
import re
import sys
import time
import json
import socket
import locale
import logging
import argparse
from http import cookiejar
from importlib import import_module
from urllib import request, parse, error

from you_get.common import urlopen_with_retry, fake_headers, force, tr, url_size


def url_save_overload(
    url, filepath, bar, refer=None, is_part=False, faker=False,
    headers=None, timeout=None, **kwargs
):
    tmp_headers = headers.copy() if headers is not None else {}
    # When a referer specified with param refer,
    # the key must be 'Referer' for the hack here
    if refer is not None:
        tmp_headers['Referer'] = refer
    file_size = url_size(url, faker=faker, headers=tmp_headers)

    if os.path.exists(filepath):
        if not force and file_size == os.path.getsize(filepath):
            if not is_part:
                if bar:
                    bar.done()
                print(
                    'Skipping {}: file already exists'.format(
                        tr(os.path.basename(filepath))
                    )
                )
            else:
                if bar:
                    bar.update_received(file_size)
            return
        else:
            if not is_part:
                if bar:
                    bar.done()
                print('Overwriting %s' % tr(os.path.basename(filepath)), '...')
    elif not os.path.exists(os.path.dirname(filepath)):
        os.mkdir(os.path.dirname(filepath))

    temp_filepath = filepath + '.download' if file_size != float('inf') \
        else filepath
    received = 0
    if not force:
        open_mode = 'ab'

        if os.path.exists(temp_filepath):
            received += os.path.getsize(temp_filepath)
            if bar:
                bar.update_received(os.path.getsize(temp_filepath))
    else:
        open_mode = 'wb'

    if received < file_size:
        if faker:
            tmp_headers = fake_headers
        '''
        if parameter headers passed in, we have it copied as tmp_header
        elif headers:
            headers = headers
        else:
            headers = {}
        '''
        if received:
            tmp_headers['Range'] = 'bytes=' + str(received) + '-'
        if refer:
            tmp_headers['Referer'] = refer

        if timeout:
            response = urlopen_with_retry(
                request.Request(url, headers=tmp_headers), timeout=timeout
            )
        else:
            response = urlopen_with_retry(
                request.Request(url, headers=tmp_headers)
            )
        try:
            range_start = int(
                response.headers[
                    'content-range'
                ][6:].split('/')[0].split('-')[0]
            )
            end_length = int(
                response.headers['content-range'][6:].split('/')[1]
            )
            range_length = end_length - range_start
        except:
            content_length = response.headers['content-length']
            range_length = int(content_length) if content_length is not None \
                else float('inf')

        if file_size != received + range_length:
            received = 0
            if bar:
                bar.received = 0
            open_mode = 'wb'

        with open(temp_filepath, open_mode) as output:
            while True:
                buffer = None
                try:
                    buffer = response.read(1024 * 256)
                except socket.timeout:
                    pass
                if not buffer:
                    if received == file_size:  # Download finished
                        break
                    # Unexpected termination. Retry request
                    tmp_headers['Range'] = 'bytes=' + str(received) + '-'
                    response = urlopen_with_retry(
                        request.Request(url, headers=tmp_headers)
                    )
                    continue
                output.write(buffer)
                received += len(buffer)
                if bar:
                    bar.update_received(len(buffer))

    assert received == os.path.getsize(temp_filepath), '%s == %s == %s' % (
        received, os.path.getsize(temp_filepath), temp_filepath
    )

    if os.access(filepath, os.W_OK):
        # on Windows rename could fail if destination filepath exists
        os.remove(filepath)
    os.rename(temp_filepath, filepath)
