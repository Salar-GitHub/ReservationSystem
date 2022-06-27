$(() => {
    $('#Repeat').change((e) => {

        if (e.currentTarget.checked) {
            $("#repeat-container").show();
        }
        else {
            $("#repeat-container").hide();
        }
    });

    $('#Start').change(() => {

        var startDate = $('#Start').val();
        const d = new Date(startDate);
        var day = d.getDay();

        for (let i = 0; i < 7; i++) {
            if (i == day) {
                $(`#Days_${i}_`).prop("checked", true);
            }
            else {
                $(`#Days_${i}_`).prop("checked", false);
            }
        };


    });
})