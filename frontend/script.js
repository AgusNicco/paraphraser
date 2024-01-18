import svc from "/services.js";

svc.pingServer();
const form = document.getElementById('paraphraseForm');
form.addEventListener('submit', async function (event) {
    event.preventDefault();
    // get user input
    const textInput = document.getElementById('textInput').value;
    const resultsContainer = document.getElementById('result');
    resultsContainer.innerHTML = 'Processing request. 10s-60s processing time.'; 
    let paraphrasedText = await svc.paraphrase(textInput);
    console.log(paraphrasedText);
    displayWithParagraphs(paraphrasedText, 'result');
});


function displayWithParagraphs(text, containerId) {
    const container = document.getElementById(containerId);
    container.innerHTML = ''; 
    container.style.display = 'block';
    const paragraphs = text.split('\n\n');
    paragraphs.forEach((paragraph, paragraphIndex) => {
        const p = document.createElement('p');
        p.textContent = paragraph; 
        container.appendChild(p); 
    });
}
