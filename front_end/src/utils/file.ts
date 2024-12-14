import datetime from '@/utils/datetime'

const fileService = {
    resolveAndDownloadBlob(byte: any, file_name: string) {
        file_name = decodeURI(file_name)
        file_name = file_name.replace(
            '.xlsx',
            `_${datetime.formatDateTimeNew(new Date().toDateString())}.xlsx`
        )
        const url = window.URL.createObjectURL(new Blob([byte]))
        const link = document.createElement('a')
        link.href = url
        link.setAttribute('download', file_name)
        document.body.appendChild(link)
        link.click()
        window.URL.revokeObjectURL(url)
        link.remove()
    },
}
export default fileService
