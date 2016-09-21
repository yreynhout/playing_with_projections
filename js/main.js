var https = require("https");
var fs = require('fs');

function fetchFromUrl(stream) {
    var options = {
        hostname: 'playing-with-projections.herokuapp.com',
        path: `/stream/${stream}`,
        method: 'GET'
    };

    return new Promise((resolve, reject) => {
        var data = '';
        var req = https.request(options, (res) => {
            res.setEncoding('utf8');
            res.on('data', (chunk) => data += chunk);
            res.on('end', () => resolve(JSON.parse(data)))
        });

        req.on('error', e => reject(e));
        req.end();
    });
}

function fetchFromFile(stream) {
  return new Promise((resolve, reject) => {
    fs.readFile(`../data/${stream}.json`, 'utf-8', (err, data) => {
      if(err) reject(err);

      resolve(JSON.parse(data));
    })
  });
}

// Write your projection here

function registeredPlayersProjection(events) {
    return events.reduce((acc, event) => {
        var payload = event.payload;
        switch (event.type) {
            case 'PlayerHasRegistered': {
                acc[payload.player_id] = {first_name: payload.first_name, last_name: payload.last_name}
                return acc;
            }
            default: return acc
        }
    }, {});
}

// Chose fetchFromUrl or fetchFromFile

fetchFromFile(2)
    .then(events => console.log(registeredPlayersProjection(events)))
    .catch(error => console.log(error));