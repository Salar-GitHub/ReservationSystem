$(() => {
    $('button[name="status"]').each(function () { // do not use lambda here, because lambda can't use $(this)
        const button = $(this);// $(this) gives you the button for current iteration of .each
        const buttonText = button.text().trim();// .trim() removes spaces around the text

        if (buttonText === 'Pending') {
            button.removeClass().addClass('btn btn-primary');
        } else if (buttonText === 'Confirmed') {
            button.removeClass().addClass('btn btn-success');
        } else if (buttonText === 'Seated') {
            button.removeClass().addClass('btn btn-secondary');
        } else if (buttonText === 'Completed') {
            button.removeClass().addClass('btn btn-dark');
        }
        else if (buttonText === 'Cancelled') {
            button.removeClass().addClass('btn btn-danger');
        }
    })
})