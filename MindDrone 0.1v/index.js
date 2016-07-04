//puerto de comunicación con unity
var socket = require('socket.io')(4567);

var video = require('./videounity');
video.inicializarvideo(socket);

var flytest = require('./dronecontrol');
flytest.inicializar(socket);

var serverns = require('./serverneurosky');
serverns.inicializarns(socket);
