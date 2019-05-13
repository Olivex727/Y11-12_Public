const express = require('express'); // import express js library
const app = express(); //create express js instance
const path = require('path'); //import path module for file locations

//lines 7-13 credit to arjunphp.com, arjun (2017), Retreived 30/4/19

// define a route to download a file
app.get('/download/:file(*)',(req, res) => {
  var file = req.params.file;
  var fileLocation = path.join('.',file);
  console.log(fileLocation);
  res.download(fileLocation, file);
});

//Host server on port 8080
app.listen(8080,() => {
  console.log(`application is running at: http://localhost:8080`);
  console.log(`Download files at: http://localhost:8080/download/[INSERT FILENAME HERE]`);
});
