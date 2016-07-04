
function inicializar(io){

var arDrone = require('ar-drone');
//crear cliente ar-drone
var cliente  = arDrone.createClient(); 
//var io = require('./index').io;

//recibir eventos emitidos en unity
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

}

exports.inicializar = inicializar; //exportando función hacia el index

