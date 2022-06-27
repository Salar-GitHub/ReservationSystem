
$(document).ready(function () {
    $('#Reservation_ReservationStatusId').change(function () {
        if ($("#Reservation_ReservationStatusId").val() == 2) {

            $("#SelectedArea").prop("disabled", false);
            $("#save").prop('disabled', false);

        } else {
            $("#SelectedArea").prop("disabled", true);
        };
        if ($("#Reservation_ReservationId").val() != 1) {
            $(".label").text("Areas");
            $(".label").css("color", "black");
        };
        if ($("#Reservation_ReservationStatusId").val() == 3) {
            if (!$('#tables').find('#table').length) {
                $("#save").prop('disabled', true);
                $("#status").text("You should first choose an area and allocate table to this reservation.");
                $("#status").css("color", "red");

            } else {
                $("#save").prop('disabled', false);
            }
        }
    });

    $('#SelectedArea').change(function () {

        if ($("#SelectedArea").val() == 1) {
            $("#main").show();
            $("#balcony").hide();
            $("#outside").hide();

        } else if ($("#SelectedArea").val() == 2) {
            $("#balcony").show();
            $("#main").hide();
            $("#outside").hide();
        } else if ($("#SelectedArea").val() == 3) {
            $("#outside").show();
            $("#main").hide();
            $("#balcony").hide();

        } else {
            $("#outside").hide();
            $("#main").hide();
            $("#balcony").hide();
        }
    });


    $(window).on('load', function () {
        if ($("#Reservation_ReservationStatusId").val() == 2) {
            //$("#Reservation_ReservationStatusId").val(1).hide();
            $("#SelectedArea").prop("disabled", false);


        } else {
            $("#SelectedArea").prop("disabled", true);
            // $(".label").text("Booking must be confirmed to select table");
        }
        if ($("#Reservation_ReservationStatusId").val() == 5) {
            $("#Reservation_ReservationStatusId").prop("disabled", true);
            $("#Reservation_Guest").prop("readonly", true);
            $("#Reservation_StartTime").prop("readonly", true);
            $("#Reservation_Duration").prop("readonly", true);
            $("#Reservation_Note").prop("readonly", true);

        };

    });

    $('#areas').click(function () {
        if ($("#Reservation_ReservationStatusId").val() == 1) {
            $(".label").text("Booking must be confirmed to select table");
            $(".label").css("color", "red");
        }
        else if ($("#Reservation_ReservationStatusId").val() == 3)  {
            $(".label").text("The table can not be changed when the reservation status is sitted");
            $(".label").css("color", "red");
        } else if ($("#Reservation_ReservationStatusId").val() == 4) {
            $(".label").text("The table can not be changed when the reservation status is completed");
            $(".label").css("color", "red");
        }

    });
  
 

});


