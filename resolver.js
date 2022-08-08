#!/usr/bin/env node

const SwaggerClient = require('swagger-client');
const util = require('util');
const fs = require('fs');

async function f1() {
    return await SwaggerClient.resolve({ url: 'https://raw.githubusercontent.com/radiantearth/stac-api-spec/main/item-search/openapi.yaml'});
}

f1().then(function(doc) {
    let json = JSON.stringify(doc.spec);
    fs.writeFile('resolved.json', json, (err) => {
        if (err) throw err;
        console.log('Data written to file');
    });
});