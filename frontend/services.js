// const wakeUp = await fetch('http://localhost:5110/');


const apiUrl = 'http://localhost:5110/paraphrase';

async function paraphrase(text) {
    const input = {
        Text: text
    };
    const response = await fetch(apiUrl, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(input)
    });
    const result = await response.json();
    console.log("Response: "+ result);
    return result;
}

const svc = {
    paraphrase
}
export default svc;
// const data = await response.json();
// console.log('Response:', data);

// console.log(await response.text());
