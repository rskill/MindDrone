
function inicializarvideo(io){

	var fs = require('fs');
	//crear subproceso
	var spawn = require('child_process').spawn;
	//var io = require('./index').io;
	var args = ['-i', 'pipe:0', '-f', 'mjpeg', '-quality', 'realtime', '-fflags','+genpts', 'pipe:1'] //argumentos para ffmpeg
	var ffmpeg = spawn('ffmpeg', args); //ejecutar comando ffmpeg con argumentos
	var arDrone = require('ar-drone');
	var cliente = arDrone.createClient(); //creacion cliente drone
	var video = cliente.getVideoStream(); //obtener video h.264 720 yuv
	var PaVEParser = require('ar-drone/lib/video/PaVEParser');
	var parser = new PaVEParser();
	//var Readable = require('stream').Readable;
	//var rs = new Readable();

	video.pipe(parser); //eliminar custom header

	ffmpeg.stdout.
		on('data', function (data) {
	  		var frame = new Buffer(data).toString('base64'); //convertir a base64 format
	 		io.sockets.emit('canvas',{data: frame}); //emitir evento con frames decodificados
	  		//console.log(data);
		})
		.on('end', function() {
	    	console.log(end);
	  	});

	parser.pipe(ffmpeg.stdin); //enviar stream sin custom header a ffmpeg
}
exports.inicializarvideo = inicializarvideo; //exportando función hacia el index
