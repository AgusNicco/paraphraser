import svc from "/services.js";

const form = document.getElementById('paraphraseForm');

// Add a submit event listener to the form
form.addEventListener('submit', async function (event) {
    // Prevent the default form submit action
    event.preventDefault();


    // Get the value of the textarea
    const textInput = document.getElementById('textInput').value;
    
    const resultsContainer = document.getElementById('result');
    resultsContainer.innerHTML = 'Processing request. Results will show up here once done. 30s-60s processing time.'; // Clear the container
    // Assign the text to a variable
    let paraphrasedText = await svc.paraphrase(textInput);

    // For demonstration: Log the variable value to the console
    console.log(paraphrasedText);

    // const outputElement = document.getElementById('result');
    displayWithParagraphs(paraphrasedText, 'result');
});


function displayWithParagraphs(text, containerId) {
    const container = document.getElementById(containerId);
    container.innerHTML = ''; // Clear the container

    // Split the text by double newlines to preserve paragraph breaks
    const paragraphs = text.split('\n\n');

    paragraphs.forEach((paragraph, paragraphIndex) => {
        // Create a paragraph element for each paragraph
        const p = document.createElement('p');
        p.textContent = paragraph; // Set text content of the paragraph
        container.appendChild(p); // Append the paragraph element to the container

        // Add a space (an extra paragraph element) between paragraphs if it's not the last one
        // if (paragraphIndex < paragraphs.length - 1) {
        //     const spacer = document.createElement('p');
        //     spacer.innerHTML = '&nbsp;';
        //     container.appendChild(spacer);
        // }
    });
}
