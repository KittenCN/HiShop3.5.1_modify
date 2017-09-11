/**
 * 手机号
 * @param mobile
 * @returns
 */
function isMobileApp(mobile) {
	var reg=/^1[0-9]{10}/;
	return reg.test(mobile);
}

/**
 * 身份证
 * @param card
 * @returns {Boolean}
 */
function isCardNoApp(card)  {     
    // 身份证号码为15位或者18位，15位时全为数字，18位前17位为数字，最后一位是    校验位，可能为数字或字符X     
    var reg = /(^\d{15}$)|(^\d{18}$)|(^\d{17}(\d|X|x)$)/;    
    return reg.test(card);
}