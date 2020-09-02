import * as React from 'react';
import 'antd/dist/antd.css';
import { Menu } from 'antd';
import { MenuFeature } from './utils';
import { Link } from 'react-router-dom';

const { SubMenu } = Menu;

interface ISideMenuItemsProps {
    path: string;
    featureList: MenuFeature[];
    isEventCreation: boolean;
}

export default class SideMenuItems extends React.Component<ISideMenuItemsProps, any> {

    rootSubmenuKeys = this.props.featureList.filter(item => item.featureName).map(t => t.id.toString());
    defaultSelectedItem = this.props.featureList.map(item => {
        return item.childFeatures.filter(t => t.redirectUrl == this.props.path).map(t => t.id.toString())
    })[0];

    state = {
        openKeys: this.props.featureList.filter(item => item.isActiveNav == true).map(t => t.id.toString()),
    };

    onOpenChange = openKeys => {
        const latestOpenKey = openKeys.find(key => this.state.openKeys.indexOf(key) === -1);
        if (this.rootSubmenuKeys.indexOf(latestOpenKey) === -1) {
            this.setState({ openKeys });
        } else {
            this.setState({
                openKeys: latestOpenKey ? [latestOpenKey] : [],
            });
        }
    };

    render() {
        let featureMenuItems = this.props.featureList.map((parentFeature) => {
            return (
                <SubMenu
                    key={parentFeature.id.toString()}
                    title={
                        <span>
                            <i className={parentFeature.redirectUrl + " fa-fw"}></i>
                            <span>{parentFeature.featureName}</span>
                        </span>
                    }>
                    {
                        parentFeature.childFeatures.map((childFeature, i) => {
                            return (<Menu.Item key={childFeature.id.toString()}>
                                <a href={(this.props.isEventCreation && childFeature.redirectUrl.indexOf("createplace") > -1) ?
                                    "/host-online" : childFeature.redirectUrl}>
                                    {childFeature.featureName}
                                </a>
                            </Menu.Item>
                            );
                        })
                    }
                </SubMenu>
            );
        });
        return (
            <Menu
                defaultOpenKeys={this.state.openKeys}
                defaultSelectedKeys={this.defaultSelectedItem}
                mode="inline"
                openKeys={this.state.openKeys}
                onOpenChange={this.onOpenChange}
                style={{ width: 256 }}
            >
                {featureMenuItems}
            </Menu>
        );
    }
}
