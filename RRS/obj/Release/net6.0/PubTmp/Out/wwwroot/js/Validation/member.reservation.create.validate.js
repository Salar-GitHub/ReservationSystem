
$(document).ready(function () {
    $('#ResCreateForm').validate({
        rules: {
            'Firstname': {
                required: true,
                minlength: 3
            },
            'LastName': {
                required: true,
                minlength: 3
            },
            'Email': {
                email:true,
                required: true
            },
            'PhoneNumber': {
                required: true,
                number: true,
                maxlength: 10
            },
            'Guest': {
                required: true,
                min: 1,
                max: 10
            },
            'StartTime': {
                required: true,
            },
            'Duration': {
                required: true
            },
            'Note': {
                required: false,
                maxlength: 150
            }
        },
        messages: {
            'FirstName': { required: "Please enter a valid name", minlength: "Please enter a valid name with more then 2 characters" },
            'LastName': { required: "Please enter a valid name", minlength: "Please enter a valid name with more then 2 characters" },
            'Email': { required: "Please add valid email" },
            'PhoneNumber': { required: "Please enter a phone number", number: "Numbers only", maxlength: "Maximum of 10 characters" },
            'Guest': { required: "Please enter the number of guest", max: "Maxium of 10 Guest per reservation, please contact the Resturant for more detail" },
            'Duration': {required:"Please select the duration of stay"},
            'StartTime': { required: "Pleae select a reservation time" },
            'Note': {required:"Max length of 150 characters"}
        },
    });

});
//https://stackoverflow.com/questions/833997/validate-that-end-date-is-greater-than-start-date-with-jquery
