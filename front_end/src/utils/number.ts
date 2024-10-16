export default {
    formatCurrency(amount: number, locale: string = 'ja-JP', currency: string = 'JPY') {
        return amount.toLocaleString(locale, { style: 'currency', currency: currency });
    },
    parseCurrency(currencyString: any) {
        const numericString = currencyString.toLocaleString().replace(/[^0-9.-]+/g, "");
        return parseFloat(numericString);
    }
};
