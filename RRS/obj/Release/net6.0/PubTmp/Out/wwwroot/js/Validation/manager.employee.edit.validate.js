$(document).ready(function () {

    $('#EmployeeFormVal').validate({
        rules: {
            'FirstName': {
                required: true,
                minlength: 2
            },
            'LastName': {
                required: true,
                minlength: 2
            },
            'PhoneNumber': {
                required: true,
                min: 10,
                max: 10,
                number: true
            },
            'TaxFileNumber': {
                required: true,
                min: 10,
                max: 10,
                number: true
            },
            
        },
            messages: {
                'FirstName': { required: "Please enter a valid name", minlength: "Please enter a valid name with more then 2 Character" },
                'LastName': { required: "Please enter a valid first Name", minlength: "Please enter a valid last name" },
                'PhoneNumber': { required: "Please enter a valid phone number", minlength: "Minimum of 10 number", number: "Must only include numercial characters" },
                'TaxFileNumber': { required: "Please enter a valid phone number", minlength: "Minimum of 10 number", number: "Must only include numercial characters" }
            }
    });
});