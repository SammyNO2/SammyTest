function calculateCOVIDPayHours(fteHours, hoursInTimeCard) {
    return fteHours - hoursInTimeCard;
}
function calculatePayHoursWithSTD(fteHours, hoursInTimeCard) {
    return fteHours / 2 - hoursInTimeCard;
}
function calculateHoursInTimeCard(fteHours, stdPayHours) {
    return fteHours - stdPayHours;
}
function calculateHoursInTimeCardWithSTD(fteHours, stdPayHours) {
    return fteHours / 2 - stdPayHours;
}
function calculateCOVIDPayHoursFromBankPay(bankPay, rateOfPay, std) {
    if (rateOfPay == 0 || rateOfPay == null) {
        alert("Error: Employee Rate of Pay is zero.")
        return 0;
    }
    if (std == 0 || std == null) {
        alert("Error: Employee STD is zero.")
        return 0;
    }
    return bankPay / rateOfPay / std;
}
function calculatePTODollars(ptoHours, rateOfPay) {
    return ptoHours * rateOfPay;
}
function calculatePTOHours(ptoDollars, rateOfPay) {
    if (rateOfPay == 0 || rateOfPay == null) {
        alert("Error: Employee Rate of Pay is zero.")
        return 0;
    }
    return ptoDollars / rateOfPay;
}
function calculatePayHoursFromPTODollars(ptoDollars, rateOfPay, std) {
    if (rateOfPay == 0 || rateOfPay == null) {
        alert("Error: Employee Rate of Pay is zero.")
        return 0;
    }
    if (std == 0 || std == null) {
        alert("Error: Employee STD is zero.")
        return 0;
    }
    return ptoDollars / (rateOfPay * (1 - std));
}

function calculateCOVIDBankPay(payHour, rateOfPay, std) {
    var bankPay = payHour * rateOfPay * std;
    alertNegCOVIDBankPaySect1(bankPay);
    return bankPay;
}

function calculatePTOPay(payHour, rateOfPay, std) {
    if (std == 0 || std == null) {
        return 0;
    }
    return payHour * rateOfPay * (1 - std);
}
function calculatePTOPayWithMax(calcPayGross, payHour, rateOfPay, std) {
    var maxPay = 1400;
    maxPay -= calcPayGross;
    var PTODollars = payHour * rateOfPay * (1 - std);
    if (PTODollars > maxPay) {
        PTODollars = maxPay;
    }
    if (std == 0 || std == null) {
        return 0;
    }
    return PTODollars;
}
function calculateGrossFromPTODollars(stdPay, std) {
    if (std == 0 || std == null) {
        alert("Error: Employee STD is zero.")
        return 0;
    }
    return stdPay / (1 - std);
}
function checkPayHoursAgainstCOVIDBank(payHours, COVIDBankHoursRemaining) {
    $('#bankExhaustedWarning').html("");
    if (payHours > COVIDBankHoursRemaining) {
        addlHours = payHours - COVIDBankHoursRemaining;
        payHours = COVIDBankHoursRemaining;
        $('#bankExhaustedWarning').html("Adding " + payHours.toFixed(2).toString() + " hours has exhausted the COVID Bank. Please distribute " + addlHours.toFixed(2).toString() +" hours into Section 2.");
    }
    return payHours;
}

function alertNegCOVIDBankPaySect1(bankPay) {
    if (bankPay < 0) {
        $('#negCOVIDBankPayGross').html("WARNING: COVID Bank Pay is negative. Please review the above information for accuracy.");
    }
    else {
        $('#negCOVIDBankPayGross').html("");
    }
}
function alertNegCOVIDBankPayWeek1(bankPay) {
    if (bankPay < 0) {
        $('#bankExhaustedWarningWeek1').html("WARNING: Week 1 COVID Pay Gross is negative. Please review the above information for accuracy.");
    }
    else {
        $('#bankExhaustedWarningWeek1').html("");
    }
}
function alertNegCOVIDBankPayWeek2(bankPay) {
    if (bankPay < 0) {
        $('#bankExhaustedWarningWeek2').html("WARNING: Week 2 COVID Pay Gross is negative. Please review the above information for accuracy.");
    }
    else {
        $('#bankExhaustedWarningWeek2').html("");
    }
}
function alertNegCOVIDBankPayTotal(bankPay) {
    if (bankPay < 0) {
        $('#bankExhaustedWarningTotal').html("WARNING: COVID Pay Total Gross is negative. Please review the above information for accuracy.");
    }
    else {
        $('#bankExhaustedWarningTotal').html("");
    }
}
/* Pay Section 1 Calculations */
/* Beginning from hours in timecard */
function calcSect1FromTimeCard() {
    //debugger;
    var fteHour = $.isNumeric($('#FTEHours').val()) ? parseFloat($('#FTEHours').val()) : 0;
    var hoursInTimeCard = $.isNumeric($('#hoursInTimeCard').val()) ? parseFloat($('#hoursInTimeCard').val()) : 0;
    var rateOfPay = $.isNumeric($('#RateOfPay').val()) ? parseFloat($('#RateOfPay').val()) : 0;
    var calcPayHour = $.isNumeric($('#STDPayHours').val()) ? parseFloat($('#STDPayHours').val()) : 0;
    var COVIDBankPayGross = $.isNumeric($('#COVIDBankPayGross').val()) ? parseFloat($('#COVIDBankPayGross').val()) : 0;
    var COVIDBankHoursRemaining = $.isNumeric($('#COVIDBankHoursRemaining').html()) ? parseFloat($('#COVIDBankHoursRemaining').html()) : 0;
    var stdThisYear = $.isNumeric(parseFloat($('#STDElection_ThisYear').val())) ? parseFloat($('#STDElection_ThisYear').val()) / 100 : 0;
    var calcPTOHours = $.isNumeric(parseFloat($('#PTOHoursPaidSection1').val())) ? parseFloat($('#PTOHoursPaidSection1').val()) : 0;
    var calcPTODollars = $.isNumeric(parseFloat($('#PTODollarsPaidSection1').val())) ? parseFloat($('#PTODollarsPaidSection1').val()) : 0;

    calcPayHour = calculateCOVIDPayHours(fteHour, hoursInTimeCard);
    calcPayHour = checkPayHoursAgainstCOVIDBank(calcPayHour, COVIDBankHoursRemaining);
    $('#STDPayHours').val(calcPayHour.toFixed(2).toString());

    COVIDBankPayGross = calculateCOVIDBankPay(calcPayHour, rateOfPay, stdThisYear);
    $('#COVIDBankPayGross').val(COVIDBankPayGross.toFixed(2).toString());

    calcPTODollars = calculatePTOPay(calcPayHour, rateOfPay, stdThisYear);
    $('#PTODollarsPaidSection1').val(calcPTODollars.toFixed(2).toString());

    calcPTOHours = calculatePTOHours(calcPTODollars, rateOfPay);
    $('#PTOHoursPaidSection1').val(calcPTOHours.toFixed(2).toString());
    calculateCOVIDPayTotalGross();
    calculateTotalPTO();
    calculatePTOWithAdjustments();
}

/* Beginning from COVID Bank Pay Hours */
function calcSect1FromPayHours() {
    //debugger;
    var fteHour = $.isNumeric($('#FTEHours').val()) ? parseFloat($('#FTEHours').val()) : 0;
    var hoursInTimeCard = $.isNumeric($('#hoursInTimeCard').val()) ? parseFloat($('#hoursInTimeCard').val()) : 0;
    var rateOfPay = $.isNumeric($('#RateOfPay').val()) ? parseFloat($('#RateOfPay').val()) : 0;
    var calcPayHour = $.isNumeric($('#STDPayHours').val()) ? parseFloat($('#STDPayHours').val()) : 0;
    var COVIDBankPayGross = $.isNumeric($('#COVIDBankPayGross').val()) ? parseFloat($('#COVIDBankPayGross').val()) : 0;
    var COVIDBankHoursRemaining = $.isNumeric($('#COVIDBankHoursRemaining').html()) ? parseFloat($('#COVIDBankHoursRemaining').html()) : 0;
    var stdThisYear = $.isNumeric(parseFloat($('#STDElection_ThisYear').val())) ? parseFloat($('#STDElection_ThisYear').val()) / 100 : 0;
    var calcPTOHours = $.isNumeric(parseFloat($('#PTOHoursPaidSection1').val())) ? parseFloat($('#PTOHoursPaidSection1').val()) : 0;
    var calcPTODollars = $.isNumeric(parseFloat($('#PTODollarsPaidSection1').val())) ? parseFloat($('#PTODollarsPaidSection1').val()) : 0;

    calcPayHour = checkPayHoursAgainstCOVIDBank(calcPayHour, COVIDBankHoursRemaining);
    $('#STDPayHours').val(calcPayHour.toFixed(2).toString());

    hoursInTimeCard = calculateHoursInTimeCard(fteHour, calcPayHour);
    $('#hoursInTimeCard').val(hoursInTimeCard.toFixed(2).toString());

    COVIDBankPayGross = calculateCOVIDBankPay(calcPayHour, rateOfPay, stdThisYear);
    $('#COVIDBankPayGross').val(COVIDBankPayGross.toFixed(2).toString());

    calcPTODollars = calculatePTOPay(calcPayHour, rateOfPay, stdThisYear);
    $('#PTODollarsPaidSection1').val(calcPTODollars.toFixed(2).toString());

    calcPTOHours = calculatePTOHours(calcPTODollars, rateOfPay);
    $('#PTOHoursPaidSection1').val(calcPTOHours.toFixed(2).toString());
    calculateCOVIDPayTotalGross();
    calculateTotalPTO();
    calculatePTOWithAdjustments();
}

/* Beginning from COVID Bank Pay */
function calcSect1FromBankPay() {
    //debugger;
    var fteHour = $.isNumeric($('#FTEHours').val()) ? parseFloat($('#FTEHours').val()) : 0;
    var hoursInTimeCard = $.isNumeric($('#hoursInTimeCard').val()) ? parseFloat($('#hoursInTimeCard').val()) : 0;
    var rateOfPay = $.isNumeric($('#RateOfPay').val()) ? parseFloat($('#RateOfPay').val()) : 0;
    var calcPayHour = $.isNumeric($('#STDPayHours').val()) ? parseFloat($('#STDPayHours').val()) : 0;
    var COVIDBankPayGross = $.isNumeric($('#COVIDBankPayGross').val()) ? parseFloat($('#COVIDBankPayGross').val()) : 0;
    var COVIDBankHoursRemaining = $.isNumeric($('#COVIDBankHoursRemaining').html()) ? parseFloat($('#COVIDBankHoursRemaining').html()) : 0;
    var stdThisYear = $.isNumeric(parseFloat($('#STDElection_ThisYear').val())) ? parseFloat($('#STDElection_ThisYear').val()) / 100 : 0;
    var calcPTOHours = $.isNumeric(parseFloat($('#PTOHoursPaidSection1').val())) ? parseFloat($('#PTOHoursPaidSection1').val()) : 0;
    var calcPTODollars = $.isNumeric(parseFloat($('#PTODollarsPaidSection1').val())) ? parseFloat($('#PTODollarsPaidSection1').val()) : 0;


    if (stdThisYear > 0) {
        calcPayHour = calculateCOVIDPayHoursFromBankPay(COVIDBankPayGross, rateOfPay, stdThisYear);
        calcPayHour = checkPayHoursAgainstCOVIDBank(calcPayHour, COVIDBankHoursRemaining);
        $('#STDPayHours').val(calcPayHour.toFixed(2).toString());

        hoursInTimeCard = calculateHoursInTimeCard(fteHour, calcPayHour);
        $('#hoursInTimeCard').val(hoursInTimeCard.toFixed(2).toString());

        COVIDBankPayGross = calculateCOVIDBankPay(calcPayHour, rateOfPay, stdThisYear);
        $('#COVIDBankPayGross').val(COVIDBankPayGross.toFixed(2).toString());

        calcPTODollars = calculatePTOPay(calcPayHour, rateOfPay, stdThisYear);
        $('#PTODollarsPaidSection1').val(calcPTODollars.toFixed(2).toString());

        calcPTOHours = calculatePTOHours(calcPTODollars, rateOfPay);
        $('#PTOHoursPaidSection1').val(calcPTOHours.toFixed(2).toString());
    }

    calculateCOVIDPayTotalGross();
    calculateTotalPTO();
    calculatePTOWithAdjustments();
}

/* Beginning from PTO Pay Hours */
function calcSect1FromPTOHours() {
    //debugger;
    var fteHour = $.isNumeric($('#FTEHours').val()) ? parseFloat($('#FTEHours').val()) : 0;
    var hoursInTimeCard = $.isNumeric($('#hoursInTimeCard').val()) ? parseFloat($('#hoursInTimeCard').val()) : 0;
    var rateOfPay = $.isNumeric($('#RateOfPay').val()) ? parseFloat($('#RateOfPay').val()) : 0;
    var calcPayHour = $.isNumeric($('#STDPayHours').val()) ? parseFloat($('#STDPayHours').val()) : 0;
    var COVIDBankPayGross = $.isNumeric($('#COVIDBankPayGross').val()) ? parseFloat($('#COVIDBankPayGross').val()) : 0;
    var COVIDBankHoursRemaining = $.isNumeric($('#COVIDBankHoursRemaining').html()) ? parseFloat($('#COVIDBankHoursRemaining').html()) : 0;
    var stdThisYear = $.isNumeric(parseFloat($('#STDElection_ThisYear').val())) ? parseFloat($('#STDElection_ThisYear').val()) / 100 : 0;
    var calcPTOHours = $.isNumeric(parseFloat($('#PTOHoursPaidSection1').val())) ? parseFloat($('#PTOHoursPaidSection1').val()) : 0;
    var calcPTODollars = $.isNumeric(parseFloat($('#PTODollarsPaidSection1').val())) ? parseFloat($('#PTODollarsPaidSection1').val()) : 0;

    calcPTODollars = calculatePTODollars(calcPTOHours, rateOfPay);
    $('#PTODollarsPaidSection1').val(calcPTODollars.toFixed(2).toString());

    if (stdThisYear > 0) {
        calcPayHour = calculatePayHoursFromPTODollars(calcPTODollars, rateOfPay, stdThisYear);
        calcPayHour = checkPayHoursAgainstCOVIDBank(calcPayHour, COVIDBankHoursRemaining);
        $('#STDPayHours').val(calcPayHour.toFixed(2).toString());

        hoursInTimeCard = calculateHoursInTimeCard(fteHour, calcPayHour);
        $('#hoursInTimeCard').val(hoursInTimeCard.toFixed(2).toString());

        COVIDBankPayGross = calculateCOVIDBankPay(calcPayHour, rateOfPay, stdThisYear);
        $('#COVIDBankPayGross').val(COVIDBankPayGross.toFixed(2).toString());

        calcPTODollars = calculatePTOPay(calcPayHour, rateOfPay, stdThisYear);
        $('#PTODollarsPaidSection1').val(calcPTODollars.toFixed(2).toString());

        calcPTOHours = calculatePTOHours(calcPTODollars, rateOfPay);
        $('#PTOHoursPaidSection1').val(calcPTOHours.toFixed(2).toString());

    }
    calculateCOVIDPayTotalGross();
    calculateTotalPTO();
    calculatePTOWithAdjustments();
}

/* Beginning from PTO Pay Dollars */
function calcSect1FromPTODollars() {
    //debugger;
    var fteHour = $.isNumeric($('#FTEHours').val()) ? parseFloat($('#FTEHours').val()) : 0;
    var hoursInTimeCard = $.isNumeric($('#hoursInTimeCard').val()) ? parseFloat($('#hoursInTimeCard').val()) : 0;
    var rateOfPay = $.isNumeric($('#RateOfPay').val()) ? parseFloat($('#RateOfPay').val()) : 0;
    var calcPayHour = $.isNumeric($('#STDPayHours').val()) ? parseFloat($('#STDPayHours').val()) : 0;
    var COVIDBankPayGross = $.isNumeric($('#COVIDBankPayGross').val()) ? parseFloat($('#COVIDBankPayGross').val()) : 0;
    var COVIDBankHoursRemaining = $.isNumeric($('#COVIDBankHoursRemaining').html()) ? parseFloat($('#COVIDBankHoursRemaining').html()) : 0;
    var stdThisYear = $.isNumeric(parseFloat($('#STDElection_ThisYear').val())) ? parseFloat($('#STDElection_ThisYear').val()) / 100 : 0;
    var calcPTOHours = $.isNumeric(parseFloat($('#PTOHoursPaidSection1').val())) ? parseFloat($('#PTOHoursPaidSection1').val()) : 0;
    var calcPTODollars = $.isNumeric(parseFloat($('#PTODollarsPaidSection1').val())) ? parseFloat($('#PTODollarsPaidSection1').val()) : 0;

    calcPTOHours = calculatePTOHours(calcPTODollars, rateOfPay);
    $('#PTOHoursPaidSection1').val(calcPTOHours.toFixed(2).toString());

    if (stdThisYear > 0) {
        calcPayHour = calculatePayHoursFromPTODollars(calcPTODollars, rateOfPay, stdThisYear);
        calcPayHour = checkPayHoursAgainstCOVIDBank(calcPayHour, COVIDBankHoursRemaining);
        $('#STDPayHours').val(calcPayHour.toFixed(2).toString());

        hoursInTimeCard = calculateHoursInTimeCard(fteHour, calcPayHour);
        $('#hoursInTimeCard').val(hoursInTimeCard.toFixed(2).toString());

        COVIDBankPayGross = calculateCOVIDBankPay(calcPayHour, rateOfPay, stdThisYear);
        $('#COVIDBankPayGross').val(COVIDBankPayGross.toFixed(2).toString());

        calcPTODollars = calculatePTOPay(calcPayHour, rateOfPay, stdThisYear);
        $('#PTODollarsPaidSection1').val(calcPTODollars.toFixed(2).toString());

    }

    calcPTOHours = calculatePTOHours(calcPTODollars, rateOfPay);
    $('#PTOHoursPaidSection1').val(calcPTOHours.toFixed(2).toString());
    calculateCOVIDPayTotalGross();
    calculateTotalPTO();
    calculatePTOWithAdjustments();
}

/* Pay Section 2 functions */
/* Beginning from hours in timecard (week 1 or week 2) */
function calcSect2FromTimeCard(el) {
    //debugger;
    var fteHour = $.isNumeric($('#FTEHours').val()) ? parseFloat($('#FTEHours').val()) : 0;
    var rateOfPay = $.isNumeric($('#RateOfPay').val()) ? parseFloat($('#RateOfPay').val()) : 0;
    var stdThisYear = $.isNumeric(parseFloat($('#STDElection_ThisYear').val())) ? parseFloat($('#STDElection_ThisYear').val()) / 100 : 0;
    var hoursInTimeCard = 0;
    var calcPayHour = 0;
    var calcPayGross = 0;
    var ptoDollars = 0;
    var ptoHours = 0;
    if (el.id == 'week1HoursInTimeCard') {
        hoursInTimeCard = $.isNumeric($('#week1HoursInTimeCard').val()) ? parseFloat($('#week1HoursInTimeCard').val()) : 0;
    }
    else {
        hoursInTimeCard = $.isNumeric($('#week2HoursInTimeCard').val()) ? parseFloat($('#week2HoursInTimeCard').val()) : 0;
    }
    calcPayHour = calculatePayHoursWithSTD(fteHour, hoursInTimeCard);
    calcPayGross = calculateCOVIDBankPay(calcPayHour, rateOfPay, stdThisYear);
    if (calcPayGross > 1400) {
        alert("Employee has hit COVID Pay Gross maximum of $1400 per week. Adjusting COVID Pay Gross to $1400 and recalculating.");
        calcPayGross = 1400;
        calcPayHour = calculateCOVIDPayHoursFromBankPay(calcPayGross, rateOfPay, stdThisYear);
    } 
    ptoDollars = calculatePTOPayWithMax(calcPayGross, calcPayHour, rateOfPay, stdThisYear);
    ptoHours = calculatePTOHours(ptoDollars, rateOfPay);

    if (el.id == 'week1HoursInTimeCard') {
        $('#week1HoursInTimeCard').val(hoursInTimeCard.toFixed(2).toString());
        $('#week1COVIDPayHours').val(calcPayHour.toFixed(2).toString());
        $('#week1COVIDPayGross').val(calcPayGross.toFixed(2).toString());
        $('#PTOHoursPaidSection2Week1').val(ptoHours.toFixed(2).toString());
        $('#PTODollarsPaidSection2Week1').val(ptoDollars.toFixed(2).toString());
        alertNegCOVIDBankPayWeek1(calcPayGross);
    }
    else {
        $('#week2HoursInTimeCard').val(hoursInTimeCard.toFixed(2).toString());
        $('#week2COVIDPayHours').val(calcPayHour.toFixed(2).toString());
        $('#week2COVIDPayGross').val(calcPayGross.toFixed(2).toString());
        $('#PTOHoursPaidSection2Week2').val(ptoHours.toFixed(2).toString());
        $('#PTODollarsPaidSection2Week2').val(ptoDollars.toFixed(2).toString());
        alertNegCOVIDBankPayWeek2(calcPayGross);
    }
    calculateCOVIDPayTotalGross();
    calculateTotalPTO();
    calculatePTOWithAdjustments();
}

/* Beginning from pay hours (week 1 or week 2) */
function calcSect2FromPayHours(el) {
    //debugger;
    var fteHour = $.isNumeric($('#FTEHours').val()) ? parseFloat($('#FTEHours').val()) : 0;
    var rateOfPay = $.isNumeric($('#RateOfPay').val()) ? parseFloat($('#RateOfPay').val()) : 0;
    var stdThisYear = $.isNumeric(parseFloat($('#STDElection_ThisYear').val())) ? parseFloat($('#STDElection_ThisYear').val()) / 100 : 0;
    var hoursInTimeCard = 0;
    var calcPayHour = 0;
    var calcPayGross = 0;
    var ptoDollars = 0;
    var ptoHours = 0;
    if (el.id == 'week1COVIDPayHours') {
        calcPayHour = $.isNumeric($('#week1COVIDPayHours').val()) ? parseFloat($('#week1COVIDPayHours').val()) : 0;
    }
    else {
        calcPayHour = $.isNumeric($('#week2COVIDPayHours').val()) ? parseFloat($('#week2COVIDPayHours').val()) : 0;
    }
    hoursInTimeCard = calculateHoursInTimeCardWithSTD(fteHour, calcPayHour);
    calcPayGross = calculateCOVIDBankPay(calcPayHour, rateOfPay, stdThisYear);
    if (calcPayGross > 1400) {
        calcPayGross = 1400;
        alert("Employee has hit COVID Pay Gross maximum of $1400 per week. Adjusting COVID Pay Gross to $1400 and recalculating.");
        calcPayHour = calculateCOVIDPayHoursFromBankPay(calcPayGross, rateOfPay, stdThisYear);
    }
    ptoDollars = calculatePTOPayWithMax(calcPayGross, calcPayHour, rateOfPay, stdThisYear);
    ptoHours = calculatePTOHours(ptoDollars, rateOfPay);

    if (el.id == 'week1COVIDPayHours') {
        $('#week1HoursInTimeCard').val(hoursInTimeCard.toFixed(2).toString());
        $('#week1COVIDPayHours').val(calcPayHour.toFixed(2).toString());
        $('#week1COVIDPayGross').val(calcPayGross.toFixed(2).toString());
        $('#PTOHoursPaidSection2Week1').val(ptoHours.toFixed(2).toString());
        $('#PTODollarsPaidSection2Week1').val(ptoDollars.toFixed(2).toString());
        alertNegCOVIDBankPayWeek1(calcPayGross);
    }
    else {
        $('#week2HoursInTimeCard').val(hoursInTimeCard.toFixed(2).toString());
        $('#week2COVIDPayHours').val(calcPayHour.toFixed(2).toString());
        $('#week2COVIDPayGross').val(calcPayGross.toFixed(2).toString());
        $('#PTOHoursPaidSection2Week2').val(ptoHours.toFixed(2).toString());
        $('#PTODollarsPaidSection2Week2').val(ptoDollars.toFixed(2).toString());
        alertNegCOVIDBankPayWeek2(calcPayGross);
    }
    calculateCOVIDPayTotalGross();
    calculateTotalPTO();
    calculatePTOWithAdjustments();
}

/* Beginning from COVID Pay (week 1 or week 2) */
function calcSect2FromCOVIDPay(el) {
    var fteHour = $.isNumeric($('#FTEHours').val()) ? parseFloat($('#FTEHours').val()) : 0;
    var rateOfPay = $.isNumeric($('#RateOfPay').val()) ? parseFloat($('#RateOfPay').val()) : 0;
    var stdThisYear = $.isNumeric(parseFloat($('#STDElection_ThisYear').val())) ? parseFloat($('#STDElection_ThisYear').val()) / 100 : 0;
    var hoursInTimeCard = 0;
    var calcPayHour = 0;
    var calcPayGross = 0;
    var ptoDollars = 0;
    var ptoHours = 0;
    if (el.id == 'week1COVIDPayGross') {
        calcPayGross = $.isNumeric($('#week1COVIDPayGross').val()) ? parseFloat($('#week1COVIDPayGross').val()) : 0;
    }
    else {
        calcPayGross = $.isNumeric($('#week2COVIDPayGross').val()) ? parseFloat($('#week2COVIDPayGross').val()) : 0;
    }
    if (calcPayGross > 1400) {
        calcPayGross = 1400;
        alert("Employee has hit COVID Pay Gross maximum of $1400 per week. Adjusting COVID Pay Gross to $1400 and recalculating.");
    }

    if (stdThisYear > 0) {
        calcPayHour = calculateCOVIDPayHoursFromBankPay(calcPayGross, rateOfPay, stdThisYear);
        hoursInTimeCard = calculateHoursInTimeCardWithSTD(fteHour, calcPayHour);
        ptoDollars = calculatePTOPayWithMax(calcPayGross, calcPayHour, rateOfPay, stdThisYear);
        ptoHours = calculatePTOHours(ptoDollars, rateOfPay);

    }

    if (el.id == 'week1COVIDPayGross') {
        if (stdThisYear > 0){
            $('#week1COVIDPayHours').val(calcPayHour.toFixed(2).toString());
            $('#week1HoursInTimeCard').val(hoursInTimeCard.toFixed(2).toString());
            $('#PTOHoursPaidSection2Week1').val(ptoHours.toFixed(2).toString());
            $('#PTODollarsPaidSection2Week1').val(ptoDollars.toFixed(2).toString());
        }
        
        $('#week1COVIDPayGross').val(calcPayGross.toFixed(2).toString());
        alertNegCOVIDBankPayWeek1(calcPayGross);

    }
    else {
        if (stdThisYear > 0) {
            $('#week2COVIDPayHours').val(calcPayHour.toFixed(2).toString());
            $('#week2HoursInTimeCard').val(hoursInTimeCard.toFixed(2).toString());
            $('#PTOHoursPaidSection2Week2').val(ptoHours.toFixed(2).toString());
            $('#PTODollarsPaidSection2Week2').val(ptoDollars.toFixed(2).toString());
        }
        $('#week2COVIDPayGross').val(calcPayGross.toFixed(2).toString());
        alertNegCOVIDBankPayWeek2(calcPayGross);

    }
    calculateCOVIDPayTotalGross();
}

/* Beginning from PTO Pay Dollars (week 1 or week 2) */
function calcSect2FromPTODollars(el) {
    //debugger;
    var fteHour = $.isNumeric($('#FTEHours').val()) ? parseFloat($('#FTEHours').val()) : 0;
    var rateOfPay = $.isNumeric($('#RateOfPay').val()) ? parseFloat($('#RateOfPay').val()) : 0;
    var stdThisYear = $.isNumeric(parseFloat($('#STDElection_ThisYear').val())) ? parseFloat($('#STDElection_ThisYear').val()) / 100 : 0;
    var hoursInTimeCard = 0;
    var calcPayHour = 0;
    var calcPayGross = 0;
    var ptoDollars = 0;
    var ptoHours = 0;
    if (el.id == 'PTODollarsPaidSection2Week1') {
        ptoDollars = $.isNumeric($('#PTODollarsPaidSection2Week1').val()) ? parseFloat($('#PTODollarsPaidSection2Week1').val()) : 0;
    }
    else {
        ptoDollars = $.isNumeric($('#PTODollarsPaidSection2Week2').val()) ? parseFloat($('#PTODollarsPaidSection2Week2').val()) : 0;
    }

    if (stdThisYear > 0) {
        calcPayGross = calculateGrossFromPTODollars(ptoDollars, stdThisYear);
        if (calcPayGross > 1400) {
            calcPayGross = 1400;
            alert("Employee has hit COVID Pay Gross maximum of $1400 per week. Adjusting COVID Pay Gross to $1400 and recalculating.");
            calcPayHour = calculateCOVIDPayHoursFromBankPay(calcPayGross, rateOfPay, stdThisYear);
            ptoDollars = calculatePTOPayWithMax(calcPayGross, calcPayHour, rateOfPay, stdThisYear);
        }
        else {
            calcPayHour = calculateCOVIDPayHoursFromBankPay(calcPayGross, rateOfPay, stdThisYear);
        }
        hoursInTimeCard = calculateHoursInTimeCardWithSTD(fteHour, calcPayHour);
    }
    ptoHours = calculatePTOHours(ptoDollars, rateOfPay);

    if (el.id == 'PTODollarsPaidSection2Week1') {
        if (stdThisYear > 0) {
            $('#week1COVIDPayHours').val(calcPayHour.toFixed(2).toString());
            $('#week1COVIDPayGross').val(calcPayGross.toFixed(2).toString());
            $('#week1HoursInTimeCard').val(hoursInTimeCard.toFixed(2).toString());
            alertNegCOVIDBankPayWeek1(calcPayGross);
        }
        $('#PTOHoursPaidSection2Week1').val(ptoHours.toFixed(2).toString());
        $('#PTODollarsPaidSection2Week1').val(ptoDollars.toFixed(2).toString());
    }
    else {
        if (stdThisYear > 0) {
            $('#week2COVIDPayHours').val(calcPayHour.toFixed(2).toString());
            $('#week2HoursInTimeCard').val(hoursInTimeCard.toFixed(2).toString());
            $('#week2COVIDPayGross').val(calcPayGross.toFixed(2).toString());
            alertNegCOVIDBankPayWeek2(calcPayGross);

        }
        $('#PTOHoursPaidSection2Week2').val(ptoHours.toFixed(2).toString());
        $('#PTODollarsPaidSection2Week2').val(ptoDollars.toFixed(2).toString());
    }
    calculateCOVIDPayTotalGross();
    calculateTotalPTO();
    calculatePTOWithAdjustments();
}

/* Beginning from PTO Pay Hours (week 1 or week 2) */
function calcSect2FromPTOHours(el) {
    //debugger;
    var fteHour = $.isNumeric($('#FTEHours').val()) ? parseFloat($('#FTEHours').val()) : 0;
    var rateOfPay = $.isNumeric($('#RateOfPay').val()) ? parseFloat($('#RateOfPay').val()) : 0;
    var stdThisYear = $.isNumeric(parseFloat($('#STDElection_ThisYear').val())) ? parseFloat($('#STDElection_ThisYear').val()) / 100 : 0;
    var hoursInTimeCard = 0;
    var calcPayHour = 0;
    var calcPayGross = 0;
    var ptoDollars = 0;
    var ptoHours = 0;
    if (el.id == 'PTOHoursPaidSection2Week1') {
        ptoHours = $.isNumeric($('#PTOHoursPaidSection2Week1').val()) ? parseFloat($('#PTOHoursPaidSection2Week1').val()) : 0;
    }
    else {
        ptoHours = $.isNumeric($('#PTOHoursPaidSection2Week2').val()) ? parseFloat($('#PTOHoursPaidSection2Week2').val()) : 0;
    }
    ptoDollars = calculatePTODollars(ptoHours, rateOfPay);
    if (stdThisYear > 0) {
        calcPayGross = calculateGrossFromPTODollars(ptoDollars, stdThisYear);
        if (calcPayGross > 1400) {
            calcPayGross = 1400;
            alert("Employee has hit COVID Pay Gross maximum of $1400 per week. Adjusting COVID Pay Gross to $1400 and recalculating.");
            calcPayHour = calculateCOVIDPayHoursFromBankPay(calcPayGross, rateOfPay, stdThisYear);
        }
        else {
            calcPayHour = calculateCOVIDPayHoursFromBankPay(calcPayGross, rateOfPay, stdThisYear);
        }
        hoursInTimeCard = calculateHoursInTimeCardWithSTD(fteHour, calcPayHour);
        ptoDollars = calculatePTOPayWithMax(calcPayGross, calcPayHour, rateOfPay, stdThisYear);
        ptoHours = calculatePTOHours(ptoDollars, rateOfPay);
    }
    if (el.id == 'PTOHoursPaidSection2Week1') {
        if (stdThisYear > 0) {
            $('#week1COVIDPayHours').val(calcPayHour.toFixed(2).toString());
            $('#week1COVIDPayGross').val(calcPayGross.toFixed(2).toString());
            $('#week1HoursInTimeCard').val(hoursInTimeCard.toFixed(2).toString());
            alertNegCOVIDBankPayWeek1(calcPayGross);
        }
        $('#PTOHoursPaidSection2Week1').val(ptoHours.toFixed(2).toString());
        $('#PTODollarsPaidSection2Week1').val(ptoDollars.toFixed(2).toString());
    }
    else {
        if (stdThisYear > 0) {
            $('#week2COVIDPayHours').val(calcPayHour.toFixed(2).toString());
            $('#week2HoursInTimeCard').val(hoursInTimeCard.toFixed(2).toString());
            $('#week2COVIDPayGross').val(calcPayGross.toFixed(2).toString());
            alertNegCOVIDBankPayWeek2(calcPayGross);
        }
        $('#PTOHoursPaidSection2Week2').val(ptoHours.toFixed(2).toString());
        $('#PTODollarsPaidSection2Week2').val(ptoDollars.toFixed(2).toString());
    }
    calculateCOVIDPayTotalGross();
    calculateTotalPTO();
    calculatePTOWithAdjustments();
}

/* Calculate COVID Pay Total Gross */
function calculateCOVIDPayTotalGross() {
    //debugger;
    var COVIDBankPayGross = $.isNumeric($('#COVIDBankPayGross').val()) ? parseFloat($('#COVIDBankPayGross').val()) : 0;
    var COVIDPayWeek1 = $.isNumeric($('#week1COVIDPayGross').val()) ? parseFloat($('#week1COVIDPayGross').val()) : 0;
    var COVIDPayWeek2 = $.isNumeric($('#week2COVIDPayGross').val()) ? parseFloat($('#week2COVIDPayGross').val()) : 0;
    var COVIDPayTotalGross = 0;
    var week1Cigna = $('#Week1CignaHidden').val()
    var week2Cigna = $('#Week2CignaHidden').val()
    if (week1Cigna == 'true') {
        COVIDPayWeek1 = 0;
    }
    if (week2Cigna == 'true') {
        COVIDPayWeek2 = 0;
    }
    COVIDPayTotalGross = COVIDBankPayGross + COVIDPayWeek1 + COVIDPayWeek2;
    alertNegCOVIDBankPayTotal(COVIDPayTotalGross)
    $('#COVIDPayTotalGross').val(COVIDPayTotalGross.toFixed(2).toString());
}

/* Calculate the total PTO Pay Hours */
function calculateTotalPTO() {
    var covidBankPTOHours = $.isNumeric($('#PTOHoursPaidSection1').val()) ? parseFloat($('#PTOHoursPaidSection1').val()) : 0;
    var week1PtoHours = $.isNumeric($('#PTOHoursPaidSection2Week1').val()) ? parseFloat($('#PTOHoursPaidSection2Week1').val()) : 0;
    var week2PtoHours = $.isNumeric($('#PTOHoursPaidSection2Week2').val()) ? parseFloat($('#PTOHoursPaidSection2Week2').val()) : 0;
    var totalPTO =  covidBankPTOHours + week1PtoHours + week2PtoHours;

    $('#PTOPayHours').val(totalPTO.toFixed(2).toString());
}

/* Calculate the PTO with adjustments */
function calculatePTOWithAdjustments() {
    var totalPTO = $.isNumeric($('#PTOPayHours').val()) ? parseFloat($('#PTOPayHours').val()) : 0;
    var addedByTK = $.isNumeric($('#PTOAddedByTimekeeper').val()) ? parseFloat($('#PTOAddedByTimekeeper').val()) : 0;
    var adjustedPTO = totalPTO - addedByTK;

    $('#TotalPTOAdjustment').val(adjustedPTO.toFixed(2).toString());
    $('#PTOAdjustment').val(adjustedPTO.toFixed(2).toString());
}