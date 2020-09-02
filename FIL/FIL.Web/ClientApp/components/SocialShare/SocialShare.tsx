import * as React from "react";
import {
  FacebookShareButton,
  LinkedinShareButton,
  TwitterShareButton,
  WhatsappShareButton,
  EmailShareButton,
  FacebookIcon,
  TwitterIcon,
  LinkedinIcon,
  WhatsappIcon,
  EmailIcon,
} from "react-share";

const COMPONENTS = {
  facebook: {
    Comp: FacebookShareButton,
    Ico: FacebookIcon,
  },
  linkedIn: {
    Comp: LinkedinShareButton,
    Ico: LinkedinIcon,
  },
  twitter: {
    Comp: TwitterShareButton,
    Ico: TwitterIcon,
  },
  whatsApp: {
    Comp: WhatsappShareButton,
    Ico: WhatsappIcon,
  },
  email: {
    Comp: EmailShareButton,
    Ico: EmailIcon,
  },
};

interface ShareProps {
  className?: string;
  disabled?: boolean;
  disabledStyle?: React.CSSProperties;
  forwardedRef?: React.Ref<HTMLButtonElement>;
  onClick?: (event: React.MouseEvent<HTMLButtonElement>, link: string) => void;
  openShareDialogOnClick?: boolean;
  url: string;
  style?: React.CSSProperties;
  windowWidth?: number;
  windowHeight?: number;
  beforeOnClick?: () => Promise<void>;
  onShareWindowClose?: () => void;
  resetButtonStyle?: boolean;
}

interface FacebookProps extends ShareProps {
  quote?: string;
  hashtag?: string;
}

interface LinkedInProps extends ShareProps {
  title?: string;
  summary?: string;
  source?: string;
}

interface TwitterProps extends ShareProps {
  title?: string;
  via?: string;
  hashtags?: string[];
  related?: string[];
}

interface WhatsappProps extends ShareProps {
  title?: string;
  separator?: string;
}
interface IProps {
  isOnlineExperience?: boolean;
  socialShareProps: ISocialShareProps;
}
export interface ISocialShareProps {
  facebook?: FacebookProps;
  linkedIn?: LinkedInProps;
  twitter?: TwitterProps;
  whatsApp?: WhatsappProps;
}

const SocialShare: React.FunctionComponent<IProps> = (props) => {
  return (
    <div
      className={`d-inline-block align-middle dropdown ${
        props.isOnlineExperience ? " fil-sharethis dropup" : "  pr-2 pl-2"
      }`}
    >
      <a
        className={`nav-link ${
          props.isOnlineExperience ? "text-body p-0" : ""
        }`}
        href="javascript:void(0)"
        id="navbarDropdown2"
        role="button"
        data-toggle="dropdown"
        aria-haspopup="true"
        aria-expanded="false"
      >
        {props.isOnlineExperience ? (
          <>
            {" "}
            <img
              src={`https://static1.feelitlive.com/images/fil-images/fil-share-icon.svg`}
              alt=""
              className="mr-2"
            />
            <u>Share</u>
          </>
        ) : (
          <i className="fa fa-share-alt-square text-success" />
        )}
      </a>
      <div
        className="dropdown-menu text-center p-2 pt-3 share-social"
        aria-labelledby="navbarDropdown2"
      >
        {Object.keys(props.socialShareProps).map((item) => {
          let Link = COMPONENTS[item];
          return (
            <Link.Comp {...props[item]}>
              <Link.Ico size={25} />
            </Link.Comp>
          );
        })}
      </div>
    </div>
  );
};

export default SocialShare;
