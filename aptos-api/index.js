let express = require('express');
let app = express();

const fetch = require('node-fetch');
const aptos = require('aptos');
let bodyParser = require('body-parser');
let urlEncodedParser = bodyParser.urlencoded({ extended: false});
const client = new aptos.AptosClient("https://fullnode.random.aptoslabs.com");
const account_details = {
    address: "YourAddress",
    publicKeyHex: "PrivateKey",
    privateKeyHex: "PublicKey",
  };
const accountAptos = aptos.AptosAccount.fromAptosAccountObject(account_details);
const payload = {
          
    function: "YourAddress::geotagrandnft::get_geolocation",
  type_arguments:[],
    arguments: [],
};

app.get('/genrandloc', async (req, res) =>{

    const eventHandle = await client.view(payload)
    res.send(JSON.stringify(eventHandle[0]))
})

app.get('/mint',urlEncodedParser, async (req, res) => {
    
    const payloadmint = {
        type: "entry_function_payload",
        function: "YourAddress::geotagrandnft::mint_geo_token",
        type_arguments: [],
        arguments: [],
    };

    const txnRequest = await client.generateTransaction(accountAptos.address(), payloadmint);
    const signedTxn = await client.signTransaction(accountAptos, txnRequest);
    
    const transactionRes = await client.submitTransaction(signedTxn);
    await client.waitForTransaction(transactionRes.hash);
    console.log(transactionRes)
    res.send("NFT Mint!" +transactionRes.hash.toString());
    
})




var server = app.listen(8080, function () {
    var port = server.address().port

    console.log("Example app listening at http://localhost:%s", port);
})