const makeRequest = (
  method: string,
  url: string,
  params?: Document | BodyInit
): Promise<{ signature: string }> =>
  new Promise((resolve, reject) => {
    const xhr = new XMLHttpRequest()
    xhr.responseType = 'json'
    xhr.open(method, url)
    xhr.onload = () =>
      xhr.status >= 200 && xhr.status < 300
        ? resolve(xhr.response)
        : reject({
            status: xhr.status,
            statusText: xhr.statusText
          })

    xhr.onerror = () =>
      reject({
        status: xhr.status,
        statusText: xhr.statusText
      })

    xhr.send(params)
  })

export const generateSignedUrl = (
  signingURL: string,
  file: File,
  includeFile: boolean
): Promise<{ signature: string }> => {
  const fd = new FormData()
  fd.append('name', file.name)
  fd.append('type', file.type)

  if (includeFile) fd.append('file', file)

  return makeRequest('POST', signingURL, fd)
}
