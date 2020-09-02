import * as React from 'react';
import BodyProvider from './Provider/BodyProvider';

function BodyRow(props: any) {
  return (
    <tr>
      <BodyProvider props={props} />
    </tr>
  );
}

export default BodyRow;
