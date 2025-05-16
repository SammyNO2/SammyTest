function disableSect2() {
    $('#Week1Cigna').prop('disabled', true);
    $('#week1HoursInTimeCard').prop('disabled', true);
    $('#week1COVIDPayHours').prop('disabled', true);
    $('#week1COVIDPayGross').prop('disabled', true);
    $('#PTOHoursPaidSection2Week1').prop('disabled', true);
    $('#PTODollarsPaidSection2Week1').prop('disabled', true);
    $('#Week2Cigna').prop('disabled', true);
    $('#week2HoursInTimeCard').prop('disabled', true);
    $('#week2COVIDPayHours').prop('disabled', true);
    $('#week2COVIDPayGross').prop('disabled', true);
    $('#PTOHoursPaidSection2Week2').prop('disabled', true);
    $('#PTODollarsPaidSection2Week2').prop('disabled', true);
}

function enableSect2() {
    $('#Week1Cigna').prop('disabled', false);
    $('#week1HoursInTimeCard').prop('disabled', false);
    $('#week1COVIDPayHours').prop('disabled', false);
    $('#week1COVIDPayGross').prop('disabled', false);
    $('#PTOHoursPaidSection2Week1').prop('disabled', false);
    $('#PTODollarsPaidSection2Week1').prop('disabled', false);
    $('#Week2Cigna').prop('disabled', false);
    $('#week2HoursInTimeCard').prop('disabled', false);
    $('#week2COVIDPayHours').prop('disabled', false);
    $('#week2COVIDPayGross').prop('disabled', false);
    $('#PTOHoursPaidSection2Week2').prop('disabled', false);
    $('#PTODollarsPaidSection2Week2').prop('disabled', false);
}


function disableAll(historic) {
    $('#hoursInTimeCard').prop('disabled', true);
    $('#STDPayHours').prop('disabled', true);
    $('#COVIDBankPayGross').prop('disabled', true);
    $('#PTODollarsPaidSection1').prop('disabled', true);
    $('#PTOHoursPaidSection1').prop('disabled', true);
    $('#Week1Cigna').prop('disabled', true);    
    $('#week1HoursInTimeCard').prop('disabled', true);
    $('#week1COVIDPayHours').prop('disabled', true);    
    $('#week1COVIDPayGross').prop('disabled', true);   
    $('#PTOHoursPaidSection2Week1').prop('disabled', true);    
    $('#PTODollarsPaidSection2Week1').prop('disabled', true);
    $('#Week2Cigna').prop('disabled', true);
    $('#week2HoursInTimeCard').prop('disabled', true);
    $('#week2COVIDPayHours').prop('disabled', true);
    $('#week2COVIDPayGross').prop('disabled', true);
    $('#PTOHoursPaidSection2Week2').prop('disabled', true);
    $('#PTODollarsPaidSection2Week2').prop('disabled', true);
    $('#COVIDPayTotalGross').prop('disabled', true);
    $('#PTOPayHours').prop('disabled', true);
    $('#PTOAddedByTimekeeper').prop('disabled', true);
    $('#TotalPTOAdjustment').prop('disabled', true);
    $('#COVIDTestPTOAdjustment').prop('disabled', true);
    if (!historic) {
        $('#hoursInTimeCard').val('');
        $('#STDPayHours').val('');
        $('#COVIDBankPayGross').val('');
        $('#PTODollarsPaidSection1').val('');
        $('#PTOHoursPaidSection1').val('');
        $('#Week1Cigna').prop('checked', false);
        $('#week1HoursInTimeCard').val('');
        $('#week1COVIDPayHours').val('');
        $('#week1COVIDPayGross').val('');
        $('#PTOHoursPaidSection2Week1').val('');
        $('#PTODollarsPaidSection2Week1').val('');
        $('#Week2Cigna').prop('checked', false);
        $('#week2HoursInTimeCard').val('');
        $('#week2COVIDPayHours').val('');
        $('#week2COVIDPayGross').val('');
        $('#PTOHoursPaidSection2Week2').val('');
        $('#PTODollarsPaidSection2Week2').val('');
        $('#COVIDPayTotalGross').val('');
        $('#PTOPayHours').val('');
        $('#PTOAddedByTimekeeper').val('');
        $('#TotalPTOAdjustment').val('');
        $('#bankExhaustedWarning').html("");
        $('#COVIDTestPTOAdjustment').val("");
        $('#covidTestWarning').html("");
    }
}

function enableAll(bankExhausted, historic) {
    if (!bankExhausted) {
        $('#hoursInTimeCard').prop('disabled', false);
        $('#STDPayHours').prop('disabled', false);
        $('#COVIDBankPayGross').prop('disabled', false);
        $('#PTODollarsPaidSection1').prop('disabled', false);
        $('#PTOHoursPaidSection1').prop('disabled', false);
    }
    if (!historic && bankExhausted) {
        $('#Week1Cigna').prop('disabled', false);
        $('#week1HoursInTimeCard').prop('disabled', false);
        $('#week1COVIDPayHours').prop('disabled', false);
        $('#week1COVIDPayGross').prop('disabled', false);
        $('#PTOHoursPaidSection2Week1').prop('disabled', false);
        $('#PTODollarsPaidSection2Week1').prop('disabled', false);
        $('#Week2Cigna').prop('disabled', false);
        $('#week2HoursInTimeCard').prop('disabled', false);
        $('#week2COVIDPayHours').prop('disabled', false);
        $('#week2COVIDPayGross').prop('disabled', false);
        $('#PTOHoursPaidSection2Week2').prop('disabled', false);
        $('#PTODollarsPaidSection2Week2').prop('disabled', false);
    }
    $('#COVIDPayTotalGross').prop('disabled', false);
    $('#PTOPayHours').prop('disabled', false);
    $('#PTOAddedByTimekeeper').prop('disabled', false);
    $('#TotalPTOAdjustment').prop('disabled', false);
    $('#COVIDTestPTOAdjustment').prop('disabled', false);
}