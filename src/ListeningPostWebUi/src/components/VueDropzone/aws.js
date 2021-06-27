const makeRequest = (method, url, params) =>
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

/**
 * @promise SignedUrlPromise
 * @fulfill {{signature: string}}
 * @reject {{status: number, statusText: string}}
 */

/**
 * @param {string} signingURL
 * @param {File} file
 * @param {boolean} includeFile
 * @returns {SignedUrlPromise}
 */
export const generateSignedUrl = (signingURL, file, includeFile) => {
  const fd = new FormData()
  fd.append('name', file.name)
  fd.append('type', file.type)

  if (includeFile) fd.append('file', file)

  return makeRequest('POST', signingURL, fd)
}
