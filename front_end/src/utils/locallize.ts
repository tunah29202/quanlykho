const tl = (...params: any): string => {
    const dataI18n = localStorage.getItem('i18n.ja') ?? '[]'
    const dataLocalize = JSON.parse(dataI18n)

    const lang = 'ja'
    const module = 'Core'

    let screen = params[0];
    let key = params[1];

    if (params.length > 1) {
        screen = params[0]
        key = params[1]
    } else {
        screen = params[0].split(".")[0]
        key = params[0].split(".")[1]
    }

    // filter
    let message = dataLocalize.find(
        (x: any) =>
            x.lang == lang && x.module == module && x.screen == screen && x.key == key
    )?.text

    if (params.length > 2) {
        params[2].forEach((element: any, index: number) => {
            message = message?.replace('{' + index + '}', element)
        })
    }
    return message ?? `${params[1]}`
}

export default tl
