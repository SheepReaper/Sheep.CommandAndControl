// Modified from Douglas Crockford's cycle.js (https://github.com/douglascrockford/JSON-js/blob/master/cycle.js)
// Ported over to TypeScript, and modified to handle the reference schema that Json.NET uses.

//https://bitbucket.org/smithkl42/jsonnetdecycle/raw/9029203b24f124171cf336c3fc6aa86425adec5f/JsonNetDecycle/JsonNetDecycle.ts

// type primitive = string | number

// type sequence = primitive[]

// type serializable = sequence | primitive | object

// interface STJobj {
//   $values: STJobj[] | number[] | string[]
// }
// const me = {} as STJobj

interface STJNode {
  $id: string
  $ref?: string
}

interface STJArray extends STJNode {
  $values: STJNode[]
}

type STJValue =
  | STJNode
  | boolean
  | Date
  | number
  | RegExp
  | string
  | number
  | string
  | boolean

function findReferences(obj: STJNode | STJNode[], catalog: STJNode[]): void {
  // The catalogObject function walks recursively through an object graph
  // looking for $id properties. When it finds an object with that property, then
  // it adds it to the catalog under that key.

  const objAsNode = obj as STJNode
  const objAsNodeArr = obj as STJNode[]

  let i: number
  if (obj && typeof obj === 'object') {
    const id: string = objAsNode.$id
    if (typeof id === 'string') {
      catalog[id] = obj
    }

    if (Object.prototype.toString.apply(obj) === '[object Array]') {
      for (i = 0; i < objAsNodeArr.length; i += 1) {
        findReferences(obj[i], catalog)
      }
    } else {
      for (const name in obj) {
        if (Object.prototype.hasOwnProperty.call(obj, name)) {
          if (typeof obj[name] === 'object') {
            findReferences(obj[name], catalog)
          }
        }
      }
    }
  }
}

function resolveReferences(obj: STJNode | STJNode[], catalog: STJNode[]) {
  let i: number, item: STJNode, name: string, id: string
  const objAsNodeArr = obj as STJNode[]
  const objAsSTJArr = obj as STJArray

  if (obj && typeof obj === 'object') {
    if (Object.prototype.toString.apply(obj) === '[object Array]') {
      for (i = 0; i < objAsNodeArr.length; i += 1) {
        item = obj[i]
        if (item && typeof item === 'object') {
          id = item.$ref
          if (typeof id === 'string') {
            obj[i] = catalog[id]
          } else {
            obj[i] = resolveReferences(item, catalog)
          }
        }
      }
    } else if (
      objAsSTJArr.$values &&
      Object.prototype.toString.apply(objAsSTJArr.$values) === '[object Array]'
    ) {
      const arr = []
      for (i = 0; i < objAsSTJArr.$values.length; i += 1) {
        item = objAsSTJArr.$values[i]
        if (item && typeof item === 'object') {
          id = item.$ref
          if (typeof id === 'string') {
            arr[i] = catalog[id]
          } else {
            arr[i] = resolveReferences(item, catalog)
          }
        } else {
          arr[i] = item
        }
      }
      obj = arr
    } else {
      for (name in obj) {
        if (Object.prototype.hasOwnProperty.call(obj, name)) {
          if (typeof obj[name] === 'object') {
            item = obj[name]
            if (item) {
              id = item.$ref
              if (typeof id === 'string') {
                obj[name] = catalog[id]
              } else {
                obj[name] = resolveReferences(item, catalog)
              }
            }
          }
        }
      }
    }
  }
  return obj
}

function getDecycledCopy(
  obj: STJValue | STJValue[],
  catalog: STJNode[]
): STJValue[] | STJValue | RegExp | STJNode | Date {
  // The createReferences function recurses through the object, producing the deep copy.
  let i: number // The loop counter
  let name: string // Property name
  let nu: STJValue[] | STJValue // The new object or array

  const objAsNode = obj as STJNode
  const objAsNodeArr = obj as STJNode[]

  switch (typeof obj) {
    case 'object':
      // typeof null === 'object', so get out if this value is not really an object.
      // Also get out if it is a weird builtin object.

      if (
        obj === null ||
        obj instanceof Boolean ||
        obj instanceof Date ||
        obj instanceof Number ||
        obj instanceof RegExp ||
        obj instanceof String
      ) {
        return obj
      }

      // If the value is an object or array, look to see if we have already
      // encountered it. If so, return a {$id:id} object. This is a hard way, O(n)
      // linear search that will get slower as the number of unique objects grows.
      // JavaScript really should have a decent dictionary or map class.
      for (i = 0; i < catalog.length; i += 1) {
        if (catalog[i] === obj) {
          return { $ref: i.toString() } as STJNode
        }
      }

      // Otherwise, accumulate the unique value and its id.
      objAsNode.$id = catalog.length.toString()
      catalog.push(objAsNode)

      // If it is an array, replicate the array.
      if (Object.prototype.toString.apply(obj) === '[object Array]') {
        nu = []
        for (i = 0; i < objAsNodeArr.length; i += 1) {
          nu[i] = getDecycledCopy(obj[i], catalog) as STJValue
        }
      } else {
        // If it is an object, replicate the object.
        nu = {} as STJValue
        for (name in obj) {
          if (Object.prototype.hasOwnProperty.call(obj, name)) {
            nu[name] = getDecycledCopy(obj[name], catalog)
          }
        }
      }
      return nu
    case 'number':
    case 'string':
    case 'boolean':
    default:
      return obj
  }
}

/** Make a deep copy of an object or array, assuring that there is at most
      one instance of each object or array in the resulting structure. The
      duplicate references (which might be forming cycles) are replaced with
      an object of the form
        {$id: id}
      where the id is a simple string
  Modified from Douglas Crockford's cycle.js (https://github.com/douglascrockford/JSON-js/blob/master/cycle.js)
  Ported over to TypeScript, and modified to handle the reference schema that Json.NET uses.
*/
function decycle(obj: STJValue): STJValue | STJValue[] {
  const catalog: STJNode[] = [] // Keep a reference to each unique object or array
  const newObj = getDecycledCopy(obj, catalog)
  return newObj
}

/** Restore an object that was reduced by decycle. Members whose values are objects of the form
       {$ref: id}
  are replaced with references to the value found by the id. This will
  restore cycles. The object will be mutated.
  So,
       var s = '{$id:1, members: [{$ref:"1"}]';
       return retrocycle(JSON.parse(s));
  produces an object containing an array which holds the object itself.
  */
function retrocycle(obj: STJNode): STJNode | STJNode[] {
  const catalog: STJNode[] = []
  findReferences(obj, catalog)
  return resolveReferences(obj, catalog)
}

const JsonNetDecycle = {
  decycle,
  retrocycle
}

export { JsonNetDecycle, JsonNetDecycle as default }
