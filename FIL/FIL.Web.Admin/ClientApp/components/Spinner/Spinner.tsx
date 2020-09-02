import * as React from 'react';
import { Spin } from 'antd';

interface ISpinnerProps {
  style?: React.CSSProperties;
}

const MenuSpinner = (
  <div className="spinner-border text-primary text-center" role="status">
    <span className="sr-only">Loading...</span>
  </div>
);

function Spinner(props: any) {
  const { style } = props;
  return (
    <div className={`text-center ${props.isShowLoadingMessage ? 'fil-admin-loader' : ''}`}>
      <Spin indicator={MenuSpinner} />
      {(props.isShowLoadingMessage) && <div>Getting things ready...</div>}
    </div>
  );
}

export default Spinner;
