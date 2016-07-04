//creando cliente de neurosky
	var io = require('socket.io')(4567);

	var clienteneuro = require('node-neurosky');

	var neurosky = clienteneuro.createClient({ 
		appName:'NodeNeuroSky',
		appKey:'0fc4141b4b45c675cc8d3a765b8d71c5bde9390'
	})

	var dataeSense;
	var concentracionv = [];
	var relajacionv = [];

//recibir evento con stream de datos
	neurosky.on('data', function (data){ //client listener
			var concentracion;
			var relajacion;
			io.emit('neurosky-blink', {data: data.blinkStrength}); //emite guiños
			io.emit('neurosky-eSense', {data: data.eSense}); //emite concentración y relajación
			io.emit('neurosky-signal', {data: data.poorSignalLevel});
		//	io.emit('neurosky-signal', {data: data.poorSignalLevel}); //emite fuerza de la señal
			//console.log(data.blinkStrength);
			
			concentracion = data.eSense.attention;
			meditacion = data.eSense.meditation;
			//console.log(data.poorSignalLevel);
			//console.log("meditacion " +  meditacion);
			console.log(concentracion);
			
			concentracionv.push(concentracion);
			relajacionv.push(meditacion);
	});

	io.on('connection', function(socket){
	socket.on('descanso', function(){
		//console.log('descanso');
		concentracionv.push("descanso");
		relajacionv.push("descanso");
	});
})
	io.on('connection', function(socket){
		socket.on('finexperimento', function(){
			for(var i = 0; i < concentracionv.length; i++){
				if(i == 0){
					console.log("concentracion");	
				}
				console.log(concentracionv[i]);
			}
			for(var i = 0; i < relajacionv.length; i++){
				if(i == 0){
					console.log("relajacion");
				}
				console.log(relajacionv[i]);
			}
			console.log('fin');
		});
		
	})

	neurosky.connect(); //conexión con el cliente
