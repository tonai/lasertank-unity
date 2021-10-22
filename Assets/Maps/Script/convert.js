#!/usr/bin/node

const fs = require('fs');

const filename = process.argv[2];
const rawdata = fs.readFileSync(filename);
const data = JSON.parse(rawdata);

function isGroundId(id) {
  return id < 10 || id === 18 || id === 19;
}

function mapGroundIds(id) {
  return isGroundId(id) ? id : 0;
}

function mapObjectsIds(id) {
  return isGroundId(id) ? -1 : id;
}

const text = JSON.stringify({
  "ground": data.map(row => ({ "row": row.map(mapGroundIds) })),
  "objects": data.map(row => ({ "row": row.map(mapObjectsIds) }))
}, null, 2);

fs.writeFile(filename, text, err => {
  if (err) {
    console.error(err);
    return;
  }
});
