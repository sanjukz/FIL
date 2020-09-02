import * as React from 'react';
import { withRouter, RouteComponentProps } from 'react-router-dom';
import { ISessionProps } from 'shared/stores/Session';

declare const window: any;

const Intercom: React.FunctionComponent<ISessionProps & RouteComponentProps> = (props) => {
  const { session, history } = props;
  history.listen((location, action) => {
    window.Intercom &&
      window.Intercom('update', {
        email: session.user.email,
        user_id: session.user.altId,
        last_request_at: new Date().getTime() / 1000
      });
  });

  React.useEffect(() => {
    window.Intercom &&
      window.Intercom('boot', {
        app_id: 'hpmkhsil',
        name: session.user.firstName,
        email: session.user.email,
        user_id: session.user.altId,
        user_hash: session.intercomHash
      });

    return function shutDownIntercom() {
      window.Intercom('shutdown');
      // TODO: Add init function that loads the Intercom script only when required.
      //delete window.Intercom;
      //delete window.intercomSettings;
    };
  }, []);
  return <></>;
};

export default withRouter(Intercom);
