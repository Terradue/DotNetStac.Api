from subprocess import Popen


def load_jupyter_server_extension(nbapp):
    """serve the supervisord with data from logs directory"""
    Popen(["/usr/bin/supervisord", "-c", "/etc/supervisor/conf.d/supervisord.conf"])