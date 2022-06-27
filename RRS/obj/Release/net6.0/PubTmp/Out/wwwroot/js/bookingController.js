$(() => {
    $("#btnAllSittings").click(() => {
        $("#divAllSittings").load("/Booking/AllSitting");
    });

    //


    $("#inquiry-form").submit((e) => {
        e.preventDefault();

        $('#selected-sitting').empty();

        $('#selected-sitting').load(`/Booking/Sittings?sittingTypeId=${$('#Description').val()}&date=${$('#SelectedDate').val()}`, () => {
            if ($('no-sittings').length !== 0) {
                $('#btnAllSittings').show();
            } else {

                $('#btnAllSittings').hide();
                $('#divAllSittings').empty();

            }
        });
    })


 
});