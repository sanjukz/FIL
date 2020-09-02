import * as S3FileUpload from 'react-s3'
var AWS = require('aws-sdk');

export const s3Prop = {
  multiple: false,
  // onStart(file) {
  //   console.log("onStart", file, file.name);
  // },
  onSuccess(ret, file) {
    console.log("onSuccess", ret, file.name);
  },
  // onError(err) {
  //   console.log("onError", err);
  // },
  onProgress({ percent }, file) {
    //console.log("onProgress", `${percent}%`, file.name);
  },
  customRequest({
    action,
    data,
    file,
    filename,
    headers,
    onError,
    onProgress,
    onSuccess,
    withCredentials
  }) {
    debugger;
    let a = 10;
    console.log(file);
    AWS.config.update({
      accessKeyId: "AKIAYD5645ZATMXE5H2H",
      secretAccessKey: "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",
      sessionToken: ""
    });

    const S3 = new AWS.S3();
    console.log("DEBUG filename", file.name);
    console.log("DEBUG file type", file.type);

    const objParams = {
      Bucket: "feelaplace-cdn",
      Key: data.path,
      Body: file,
      partSize: 10 * 1024 * 1024, queueSize: 1,
      ContentType: file.type // TODO: You should set content-type because AWS SDK will not automatically set file MIME
    };
    let props = data;
    data.onUploadRequest(file);
    new AWS.S3.ManagedUpload({
      partSize: 20 * 1024 * 1024, queueSize: 1,
      params: objParams
    })
      .on("httpUploadProgress", function ({ loaded, total }) {
        props.onProgress(Math.round((loaded / total) * 100))
        onProgress(
          {
            percent: Math.round((loaded / total) * 100)
          },
          file
        );
      })
      .send((err, data: any) => {
        if (err) {
          onError();
          console.log("Something went wrong");
          console.log(err.code);
          console.log(err.message);
        } else {
          props.onUploadSuccess();
          onSuccess(data.response, file);
          console.log("SEND FINISHED");
        }
      });
  }
};



export enum S3_ACTION {
  upload = 1,
  delete
}

export const s3Config = {
  bucketName: 'feelaplace-cdn',
  dirName: '',
  region: 'us-west-2',
  accessKeyId: 'AKIAYD5645ZATMXE5H2H',
  secretAccessKey: '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'
}

export const getS3Config = () => {
  return s3Config;
}

export const uploadImage = (imageFile: any, altId: string) => {
  let imageDetail = imageFile;
  var blob = imageDetail.file.slice(0, imageDetail.file.size, 'image/png')
  let filePath = `${altId}.jpg`
  var newFile = new File([blob], `${filePath}`, { type: 'image/png' })
  uploadFile(imageDetail.path, newFile, () => { (e) => { } })
}

export const uploadFile = (path, file, callback: (any) => void) => {
  const config = getS3Config();
  config.dirName = path;
  S3FileUpload.uploadFile(file, config)
    .then((data) => {
      callback(data);
      console.log(data)
    })
    .catch((err) => {
      callback(false);
    })
}

export const deleteFile = (path, file, callback: (any) => void) => {
  const config = getS3Config();
  config.dirName = path;
  S3FileUpload.deleteFile(file, config)
    .then((data) => {
      callback(data);
      console.log(data)
    })
    .catch((err) => {
      callback(false);
    })
}
