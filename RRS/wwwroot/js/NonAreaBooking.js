
$(document).ready(function () {

    $('#SelectedDate').change(function () {
        $('#Sitting').load('@Url.Action("Sitting")',"");
       
    }).trigger();

});
