import * as React from 'react';
import gtmParts from 'react-google-tag-manager';

interface IGoogleTagManagerProps {
    gtmId: string;
}

class GoogleTagManager extends React.Component<IGoogleTagManagerProps> {
    public componentDidMount() {
        if (window && !window['dataLayer']) {
            const gtmScriptNode = document.getElementById('react-google-tag-manager-gtm');
            eval(gtmScriptNode.textContent);
        }
    }

    public render() {
        if (!this.props.gtmId || this.props.gtmId.length === 0) {
            return undefined;
        }
        const gtm = gtmParts({
            id: this.props.gtmId,
            dataLayerName: 'dataLayer',
            additionalEvents: {},
            previewVariables: false,
        });
        // TODO: XXX: Put this back
        return (
            <div>
                <div>{gtm.noScriptAsReact()}</div>
                <div id="react-google-tag-manager-gtm">
                    {gtm.scriptAsReact()}
                </div>
            </div>
        );
    }
}

export default GoogleTagManager;