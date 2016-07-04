
function inicializar(io){
//abrir un puerto para el server
	//var io = require('./index').io;

	//creando cliente de neurosky
	var clienteneuro = require('node-neurosky');

	var neurosky = clienteneuro.createClient({ 
		appName:'NodeNeuroSky',
		appKey:'0fc4141b4b45c675cc8d3a765b8d71c5bde9390'
	})

//recibir evento con stream de datos
	neurosky.on('data', function (data){ //client listener
			io.emit('neurosky-blink', {data: data.blinkStrength}); //emite guiños
			io.emit('neurosky-eSense', {data: data.eSense}); //emite concentración y relajación
			io.emit('neurosky-signal', {data: data.poorSignalLevel}); //emite fuerza de la señal
			console.log(data);
	});


	neurosky.connect(); //conexión con el cliente
}

exports.inicializarns = inicializar; //exportando función hacia el index





