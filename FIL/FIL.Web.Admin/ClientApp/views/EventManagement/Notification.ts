/*Third Party Imports */
import * as React from 'react'
import { notification } from 'antd'

export const showNotification = (type, message) => {
  notification[type]({
    message: message,
    duration: 10,
    top: 10
  });
};

