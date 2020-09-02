import * as React from 'react';
import HeaderProvider from './Provider/HeaderProvider';

function HeaderRow(props: any) {
  return (
    <tr className="bg-light">
      <HeaderProvider props={props} />
    </tr>
  );
}

export default HeaderRow;
