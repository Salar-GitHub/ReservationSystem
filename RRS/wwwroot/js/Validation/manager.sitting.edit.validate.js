////$(document).ready(function () {

////    console.log("check if running");

////    jQuery.validator.addMethod("greaterThan", function (value, element, params) {
////        if (!/Invalid|NaN/.test(new Date(value))) {
////            return new Date(value) > new Date($(params).val());
////        }
////        return isNaN(value) && isNaN($(params).val()) || (Number(value) > Number($(params).val()));
////    }, 'Must be greater then {0}.');


////    jQuery.validator.addMethod("validateStartDate", function (value, element, params) {
////        var dateTime = new Date(value)
////        return (dateTime >= new Date())
////    });


////    $('#SittingFormVal').validate({
////        rules: {
////            'Name': {
////                required: true,
////                minlength: 1
////            },
////            'Start': {
////                required: true,
////                date: true,
////                validateStartDate: true
////            },
////            'End': {
////                required: true,
////                date: true,
////                greaterThan: "#Start"
////            },
////            'Capacity': {
////                required: true,
////                number: true,
////                min: 10,
////                max: 60,
////            },
////            'SittingTypeId': {
////                required: true
////            }
////        },

////        messages: {
////            'Name': { required: "Please enter a valid name", minlength: "Please enter a valid name with more then 1 character" },
////            'Start': { required: "Please enter a valid date", date: "Please enter a valid date", validateStartDate: "Start date and time must be greater or equal to today's date" },
////            'End': { required: "Please enter a valid date", date: "Please enter a valid date", greaterThan: "Please enter a DateTime thats greater than the selected start time" },
////            'Capacity': { required: "Please enter a valid amount", min: "Please enter a minium of 10", max: "Please enter a maximum of 60", number: "Numbers Only" },
////            'SittingTypeId': { required: "Please select a menu sitting from the select list" }

////        },
////    });

////});
//////https://stackoverflow.com/questions/833997/validate-that-end-date-is-greater-than-start-date-with-jquery