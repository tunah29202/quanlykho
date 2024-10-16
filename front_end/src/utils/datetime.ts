export default {
    formatDate(input: string) {
    const date = new Date(input);
    if (!isNaN(date.getTime())) {
        const m = (date.getMonth() + 1).toString().padStart(2, '0');
        const d = date.getDate().toString().padStart(2, '0');
        return `${date.getFullYear()}-${m}-${d}`;
    }
    },

    formatDateTime(input: string) {
        return input
            ? new Date(input).toISOString().replace(/T/, ' ').replace(/\..+/, '')
            : null
    },

    formatDateTimeNew(input: string) {
        const date = new Date(input)
        const year = date.getFullYear()
        const month = String(date.getMonth() + 1).padStart(2, '0')
        const day = String(date.getDate()).padStart(2, '0')

        return `${year}${month}${day}`
    },
}