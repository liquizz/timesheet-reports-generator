(function() {
    const pageTitle = document.title;

    const parts = pageTitle.split(' ');
    
    const title = parts[1];

    const description = pageTitle.substring(pageTitle.indexOf(title) + title.length + 1).trim();

    const timeSpent = document.querySelector('#__bolt-Completed-Work-input').value;
    
    // console.log('Data extracted:', { title, description });
    
    // Store the data in local storage
    chrome.storage.local.set({ ticketData: { title: title, description: description, timeSpent: timeSpent } }, function() {
        console.log('Ticket data stored.');
    });
})();
