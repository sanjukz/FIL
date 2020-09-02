import * as React from 'react';
import { Link } from 'react-router-dom';
import { getMyAccountLink } from './utils';
import "../AuthedNavMenu.scss";

const AuthItems: React.FC<any> = (props: any) => {


  const [menuOpen, setMenuOpen] = React.useState(false);

  const toggleMenu = () => {
    let getElement = document.getElementById("fil-collapse-left-menu");
    if (getElement) {
      //remove class if already present
      setMenuOpen(false)
      let currentClass = getElement.className;
      if (currentClass.indexOf("d-none") > -1) {
        setMenuOpen(true)
        getElement.classList.remove('d-none');
      } else {
        getElement.classList.add('d-none')
      }
    }
  }
  return (
    <div className="fil-admin-right-nav">
      <div className="dropdown d-inline-block">
        <a href="#" id="dropdownMenu2" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
          <img
            src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/admin-images/icons/fil-admin-profile.svg"
            width="30"
            alt="fil profile"
          />
        </a>
        <div className="dropdown-menu dropdown-menu-right" aria-labelledby="dropdownMenu2">
          <button className="dropdown-item border-bottom pb-2" type="button">
            {`${props.session.user.firstName} ${props.session.user.lastName} (${props.session.user.email})`}
          </button>
          <Link
            to={{ pathname: getMyAccountLink() }}
            target="_blank"
            className="dropdown-item border-bottom pb-2"
            type="button"
          >
            My account
        </Link>
          <a className="dropdown-item" href="/login" type="button" onClick={props.logout}>
            Sign out
        </a>
        </div>
      </div>
      <a href="javascript:void(0)" onClick={() => toggleMenu()}>
        <img src={`https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/admin-images/${menuOpen ? 'fil-menu-cross' : 'fil-menu-icon'}.svg`} className="d-none mob-menu" width="35" />
      </a>
    </div>
  );
}

export default AuthItems;
