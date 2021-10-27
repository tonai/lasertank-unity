#!/usr/bin/node

const fs = require('fs');

const filename = process.argv[2];
const rawdata = fs.readFileSync(filename);
const data = JSON.parse(rawdata);

function isFloorId(id) {
  return (id >= 2 && id <= 5) || (id >= 10 && id <= 17) || (id >= 20 && id <= 27);
}

function isGroundId(id) {
  return id < 20 && !isFloorId(id);
}

function isBlockId(id) {
  return id > 20 && !isFloorId(id);
}

function mapGroundIds(id) {
  return isGroundId(id) ? id : 0;
}

function mapFloorIds(id) {
  return isFloorId(id) ? id : -1;
}

function mapObjectsIds(id) {
  return isBlockId(id) ? id : -1;
}

const text = JSON.stringify({
  "ground": data.map(row => ({ "row": row.map(mapGroundIds) })),
  "floor": data.map(row => ({ "row": row.map(mapFloorIds) })),
  "objects": data.map(row => ({ "row": row.map(mapObjectsIds) }))
}, null, 2);

fs.writeFile(filename, text, err => {
  if (err) {
    console.error(err);
    return;
  }
});
