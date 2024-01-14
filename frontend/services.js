const apiUrl = 'https://paraphraser.azurewebsites.net/';

async function paraphrase(text) {
    const input = {
        Text: text
    };
    const response = await fetch(apiUrl+'/paraphrase', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(input)
    });
    const result = await response.json();
    console.log("Response: "+ result);
    return result.text;
}

async function pingServer() {
    const response = await fetch(apiUrl);
    if (response.ok) {
        console.log("Server ready.");
    }
}

const svc = {
    paraphrase,
    pingServer
}
export default svc;
