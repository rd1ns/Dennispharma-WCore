<?php
use \DrewM\MailChimp\MailChimp;
if ( isset( $_POST['url'] ) ) {

	$email = $_POST['url'];

	if ( ! empty( $email ) && ! filter_var( $email, FILTER_VALIDATE_EMAIL ) === false ) {
		include('MailChimp.php');

		// MailChimp API credentials
		$list_id     = 'xxxxxxxxxx';
		$api_key     = 'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx-xxxx';
		$doubleOptIn = true;

		//CONFIGURE VARIABLES START.
		$MailChimp = new MailChimp( $api_key );
		//CONFIGURE VARIABLES FINISH.

		//MAKE API CALLSE FOR MAILCHIMP START.
		$result = $MailChimp->post("lists/$list_id/members", [
			'email_address' => $email,
			'status'        => $doubleOptIn ? 'pending' : 'subscribed',
		]);
		//MAKE API CALLSE FOR MAILCHIMP FINISH.

		//CONFIGURE ERROR OR SUCCESS PROCESS START.
		if ( $MailChimp->success() ) {
			$msg = '<p class="mc-success-notice" style="color: #34A853">'."Successfully Subscribed. Please check confirmation email.".'</p>';
		} else {
			$last_response = $MailChimp->getLastResponse();
			$last_response_body = $last_response['body'];
			$last_response_data = json_decode( $last_response_body, true );

			if( $last_response_data['title'] == 'Member Exists' && strpos($last_response_data['detail'], 'Use PUT to insert or update list members.') !== false ) {
				$last_response_data['detail'] = str_replace( 'Use PUT to insert or update list members.', '', $last_response_data['detail'] );
				$last_response_data['detail'] = trim( $last_response_data['detail'] );
			}

			$msg = '<span class="mc-error-notice">' . $last_response_data['title'] .': ' . $last_response_data['detail'];
		}

		echo json_encode( $msg );

		die();
	}
}
