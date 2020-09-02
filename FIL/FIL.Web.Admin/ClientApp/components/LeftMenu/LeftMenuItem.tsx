import * as React from 'react';
import { withRouter, Link } from 'react-router-dom';

function LeftMenuItem(props: any) {
  const { menuItem } = props;
  const width = (menuItem.name == "Dashboard" || menuItem.name == "Create New Experience ") ? "16" : "14";
  return (
    <a href={menuItem.path} className={`pb-3 ${props.location.pathname == menuItem.path ? 'active' : ''}`}>
      <span className="nav-icon">
        <img
          src={`https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/admin-images/icons/${menuItem.icon}${props.location.pathname == menuItem.path ? '-active' : ''}.svg`}
          alt={`${menuItem.name} Menu Icon`}
          width={width}
        />
      </span>
      {menuItem.name}
    </a>
  );
}

export default withRouter(LeftMenuItem);
