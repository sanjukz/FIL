export const formatAddress = (addStr) => {
	var str = addStr.replace(/[ ]/g, ' ').split(',');
	var result = [];
	for (var i = 0; i < str.length; i++) {
		if (result.indexOf(str[i]) === -1) result.push(str[i]);
	}
	 return result.join(',');
};
