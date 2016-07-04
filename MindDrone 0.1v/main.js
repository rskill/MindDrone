var fs = require('fs');
var spawn = require('child_process').spawn;
var io = require('socket.io')(4567);
var args = ['-i', 'pipe:0', '-f', 'mjpeg', '-quality', 'realtime', '-fflags','+genpts', 'pipe:1']
var ffmpeg = spawn('ffmpeg', args); 
var arDrone = require('ar-drone');
var cliente = arDrone.createClient();
var video = cliente.getVideoStream();
var PaVEParser = require('ar-drone/lib/video/PaVEParser');
var parser = new PaVEParser();
var Readable = require('stream').Readable;
var rs = new Readable();

video.pipe(parser);

ffmpeg.stdout.
	on('data', function (data) {
  		var frame = new Buffer(data).toString('base64');
 		io.sockets.emit('canvas',{data: frame});
  		//console.log(frame);
	})
	.on('end', function() {
    	console.log(end);
  	});
parser.pipe(ffmpeg.stdin);

//var io= require('socket.io')(4567);

	var clienteneuro = require('node-neurosky');

	var neurosky = clienteneuro.createClient({ 
		appName:'NodeNeuroSky',
		appKey:'0fc4141b4b45c675cc8d3a765b8d71c5bde9390'
	})

//recibir el evento 
	neurosky.on('data', function (data){ //client listener
			io.emit('neurosky-blink', {data: data.blinkStrength});
			io.emit('neurosky-eSense', {data: data.eSense});
			io.emit('neurosky-signal', {data: data.poorSignalLevel});
		//	console.log(data);
	});


	neurosky.connect();

//var arDrone = require('ar-drone');
//var clientedrone  = arDrone.createClient();

io.on('connection', function(socket){
	socket.on('despegar', function(){
		cliente.takeoff();
		console.log('despegar');
	})
	.on('aterrizar', function(){
		cliente.land();
		console.log('despegar')
	});
})


